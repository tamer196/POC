using Microsoft.EntityFrameworkCore;
using POC.API.Common;
using POC.Application.Auth.Login;
using POC.Application.Journal.DTOs;
using POC.Application.Journal.Interfaces;
using POC.Persistence;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace POC.API.Middleware
{
    public class ApiJournalMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiJournalMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, AppDbContext db, IJournalRepository journalRepository)
        {
            var endpoint = context.GetEndpoint();

            if (endpoint?.Metadata.GetMetadata<ISkipJournal>() != null)
            {
                await _next(context);
                return;
            }

            var httpMethod = context.Request.Method;

            var isCreate = httpMethod.Equals("POST", StringComparison.OrdinalIgnoreCase);
            var isUpdate =
                httpMethod.Equals("PUT", StringComparison.OrdinalIgnoreCase) ||
                httpMethod.Equals("PATCH", StringComparison.OrdinalIgnoreCase);

            var entityName = GetEntityName(context);
            var entityId = GetEntityId(context);

            Dictionary<string, object>? oldEntityValues = null;
            string? requestBody = null;

            if ((isUpdate || isCreate))
            {
                requestBody = await ReadRequestBody(context);
            }

            if (isUpdate && entityId != null)
            {
                var oldEntity = await LoadOldEntity(db, entityName!, entityId);
                oldEntityValues = ConvertObjectToDictionary(oldEntity);
            }

            await _next(context);

            string? userName = null;
            string? email = null;
            Guid? userId = null;

            if (context.User?.Identity?.IsAuthenticated == true)
            {
                userName = context.User.FindFirst(ClaimTypes.Name)?.Value;
                email = context.User.FindFirst(ClaimTypes.Email)?.Value;

                var idClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (Guid.TryParse(idClaim, out var parsed))
                    userId = parsed;
            }

            Dictionary<string, object>? oldValues = null;
            Dictionary<string, object>? newValues = null;

            var requestValues = requestBody != null
                ? JsonSerializer.Deserialize<Dictionary<string, object>>(
                    requestBody,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                : null;

            oldEntityValues = ConvertJsonElements(oldEntityValues);
            requestValues = ConvertJsonElements(requestValues);

            if (isUpdate)
            {
                var changes = GetChangedValues(oldEntityValues, requestValues);
                oldValues = changes.OldValues;
                newValues = changes.NewValues;
            }
            else if (isCreate)
            {
                newValues = requestValues;
            }

            if (oldValues == null && newValues == null)
                return;

            await journalRepository.AddAsync(new JournalEntryDto
            {
                UserId = userId,
                UserName = userName,
                Email = email,
                Endpoint = context.Request.Path,
                Method = httpMethod,
                IpAddress = context.Connection.RemoteIpAddress?.ToString(),
                Timestamp = DateTime.UtcNow,
                Entity = entityName,
                EntityId = entityId,
                OldValues = oldValues,
                NewValues = newValues
            });
        }

        private async Task<string?> ReadRequestBody(HttpContext context)
        {
            context.Request.EnableBuffering();

            using var reader = new StreamReader(
                context.Request.Body,
                leaveOpen: true);

            var body = await reader.ReadToEndAsync();

            context.Request.Body.Position = 0;

            return body;
        }

        private string? GetEntityName(HttpContext context)
        {
            var segments = context.Request.Path.Value?.Split('/', StringSplitOptions.RemoveEmptyEntries);

            if (segments == null || segments.Length < 2)
                return null;

            return segments[1]; // "users"
        }

        private string? GetEntityId(HttpContext context)
        {
            if (context.Request.RouteValues.TryGetValue("id", out var id))
                return id?.ToString();

            return null;
        }

        private async Task<object?> LoadOldEntity(
     AppDbContext db,
     string entityName,
     string id)
        {
            // Find DbSet property matching route name
            var dbSetProperty = db.GetType()
                .GetProperties()
                .FirstOrDefault(p =>
                    p.PropertyType.IsGenericType &&
                    p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>) &&
                    p.Name.Equals(entityName, StringComparison.OrdinalIgnoreCase));

            if (dbSetProperty == null)
                return null;

            var entityType = dbSetProperty.PropertyType.GetGenericArguments()[0];

            object key;

            if (Guid.TryParse(id, out var guid))
                key = guid;
            else
                key = id;

            var dbSet = dbSetProperty.GetValue(db);

            var findAsync = dbSet!.GetType()
                .GetMethod("FindAsync", new[] { typeof(object[]) });

            var result = findAsync!.Invoke(dbSet, new object[] { new object[] { key } });

            // result is ValueTask<TEntity>
            dynamic valueTask = result!;

            var entity = await valueTask;

            return (object?)entity;
        }

        private Dictionary<string, object>? ConvertJsonElements(Dictionary<string, object>? source)
        {
            if (source == null)
                return null;

            var result = new Dictionary<string, object>();

            foreach (var kv in source)
            {
                if (kv.Value is JsonElement element)
                {
                    result[kv.Key] = ConvertJsonElement(element);
                }
                else
                {
                    result[kv.Key] = kv.Value!;
                }
            }

            return result;
        }

        private Dictionary<string, object>? ConvertObjectToDictionary(object? source)
        {
            if (source == null)
                return null;

            var result = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            var properties = source.GetType().GetProperties();

            foreach (var prop in properties)
            {
                // Skip navigation / complex properties
                if (prop.PropertyType != typeof(string) &&
                    !prop.PropertyType.IsValueType &&
                    !IsSimpleType(prop.PropertyType))
                {
                    continue;
                }

                var value = prop.GetValue(source);
                result[prop.Name] = NormalizeValue(value)!;
            }

            return result;
        }

        private bool IsSimpleType(Type type)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;

            return type.IsPrimitive
                || type.IsEnum
                || type == typeof(string)
                || type == typeof(decimal)
                || type == typeof(Guid)
                || type == typeof(DateTime)
                || type == typeof(DateTimeOffset)
                || type == typeof(TimeSpan);
        }

        private (Dictionary<string, object>? OldValues, Dictionary<string, object>? NewValues)
GetChangedValues(
    Dictionary<string, object>? oldValues,
    Dictionary<string, object>? requestValues)
        {
            if (oldValues == null || requestValues == null)
                return (null, null);

            var changedOldValues = new Dictionary<string, object>();
            var changedNewValues = new Dictionary<string, object>();

            var entityKeyLookup = oldValues.Keys
                .ToDictionary(k => k.ToLowerInvariant(), k => k);

            foreach (var requestItem in requestValues)
            {
                var requestKey = requestItem.Key.ToLowerInvariant();

                if (!entityKeyLookup.TryGetValue(requestKey, out var entityKey))
                    continue;

                var oldValue = oldValues[entityKey];
                var newValue = requestItem.Value;

                oldValue = NormalizeValue(oldValue);
                newValue = NormalizeValue(newValue);

                if (!Equals(oldValue, newValue))
                {
                    changedOldValues[entityKey] = oldValue!;
                    changedNewValues[entityKey] = newValue!;
                }
            }

            return (
                changedOldValues.Any() ? changedOldValues : null,
                changedNewValues.Any() ? changedNewValues : null
            );
        }

        private object? NormalizeValue(object? value)
        {
            if (value == null)
                return null;

            if (value is JsonElement element)
            {
                return element.ValueKind switch
                {
                    JsonValueKind.String => element.GetString(),
                    JsonValueKind.Number => element.TryGetInt64(out var l) ? l : element.GetDouble(),
                    JsonValueKind.True => true,
                    JsonValueKind.False => false,
                    JsonValueKind.Null => null,
                    _ => element.ToString()
                };
            }

            if (value is Guid guid)
                return guid.ToString();

            if (value is string s)
            {
                if (Guid.TryParse(s, out var parsed))
                    return parsed.ToString();

                return s;
            }

            if (value.GetType().IsEnum)
                return Convert.ToInt32(value);

            return value;
        }

        private void CompareObjects(
    string prefix,
    object? oldValue,
    object? newValue,
    Dictionary<string, object> oldChanges,
    Dictionary<string, object> newChanges)
        {
            if (oldValue == null && newValue == null)
                return;

            if (oldValue == null || newValue == null)
            {
                oldChanges[prefix] = oldValue!;
                newChanges[prefix] = newValue!;
                return;
            }

            oldValue = NormalizeValue(oldValue);
            newValue = NormalizeValue(newValue);

            var oldType = oldValue.GetType();
            var newType = newValue.GetType();

            if (IsSimpleType(oldType) && IsSimpleType(newType))
            {
                if (!Equals(oldValue, newValue))
                {
                    oldChanges[prefix] = oldValue!;
                    newChanges[prefix] = newValue!;
                }
                return;
            }

            if (oldValue is Dictionary<string, object> oldDict &&
                newValue is Dictionary<string, object> newDict)
            {
                var keys = oldDict.Keys.Union(newDict.Keys);

                foreach (var key in keys)
                {
                    oldDict.TryGetValue(key, out var o);
                    newDict.TryGetValue(key, out var n);

                    var path = string.IsNullOrEmpty(prefix)
                        ? key
                        : $"{prefix}.{key}";

                    CompareObjects(path, o, n, oldChanges, newChanges);
                }

                return;
            }

            if (oldValue is IEnumerable<object> oldList &&
                newValue is IEnumerable<object> newList)
            {
                var oldArray = oldList.ToList();
                var newArray = newList.ToList();

                var max = Math.Max(oldArray.Count, newArray.Count);

                for (int i = 0; i < max; i++)
                {
                    var path = $"{prefix}[{i}]";

                    var o = i < oldArray.Count ? oldArray[i] : null;
                    var n = i < newArray.Count ? newArray[i] : null;

                    CompareObjects(path, o, n, oldChanges, newChanges);
                }

                return;
            }

            if (!Equals(oldValue, newValue))
            {
                oldChanges[prefix] = oldValue!;
                newChanges[prefix] = newValue!;
            }
        }
    

    private object? ConvertJsonElement(JsonElement element)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.Object:
                    var dict = new Dictionary<string, object>();

                    foreach (var prop in element.EnumerateObject())
                        dict[prop.Name] = ConvertJsonElement(prop.Value)!;

                    return dict;

                case JsonValueKind.Array:
                    var list = new List<object>();

                    foreach (var item in element.EnumerateArray())
                        list.Add(ConvertJsonElement(item)!);

                    return list;

                case JsonValueKind.String:
                    return element.GetString();

                case JsonValueKind.Number:
                    return element.TryGetInt64(out var l) ? l : element.GetDouble();

                case JsonValueKind.True:
                    return true;

                case JsonValueKind.False:
                    return false;

                case JsonValueKind.Null:
                    return null;

                default:
                    return element.ToString();
            }
        }
    }
}
