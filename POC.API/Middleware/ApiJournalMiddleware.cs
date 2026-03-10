using POC.Application.Auth.Login;
using POC.Application.Journal.DTOs;
using POC.Application.Journal.Interfaces;
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

        public async Task InvokeAsync(
            HttpContext context,
            IJournalRepository journalRepository)
        {
            if (context.Request.Path.StartsWithSegments("/api/journal"))
            {
                await _next(context);
                return;
            }

            string? userName = null;
            string? email = null;
            Guid? userId = null;

            // Capture response body
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            if (context.User?.Identity?.IsAuthenticated == true)
            {
                userName = context.User.FindFirst(ClaimTypes.Name)?.Value;
                email = context.User.FindFirst(ClaimTypes.Email)?.Value;

                var idClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (Guid.TryParse(idClaim, out var parsed))
                    userId = parsed;
            }

            await _next(context);

            // Read login response and extract userId from JWT
            if (context.Request.Path.StartsWithSegments("/api/auth/login"))
            {
                responseBody.Position = 0;
                var responseText = await new StreamReader(responseBody).ReadToEndAsync();

                if (!string.IsNullOrWhiteSpace(responseText))
                {
                    using var document = JsonDocument.Parse(responseText);

                    if (document.RootElement.TryGetProperty("accessToken", out var tokenElement))
                    {
                        var accessToken = tokenElement.GetString();

                        if (!string.IsNullOrWhiteSpace(accessToken))
                        {
                            var handler = new JwtSecurityTokenHandler();
                            var token = handler.ReadJwtToken(accessToken);

                            var idClaim = token.Claims
                                .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

                            if (Guid.TryParse(idClaim, out var parsed))
                                userId = parsed;

                            userName = token.Claims
                                .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName)?.Value;

                            email = token.Claims
                                .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value;
                        }
                    }
                }

                responseBody.Position = 0;
                await responseBody.CopyToAsync(originalBodyStream);
            }
            else
            {
                responseBody.Position = 0;
                await responseBody.CopyToAsync(originalBodyStream);
            }

            var entry = new JournalEntryDto
            {
                UserId = userId,
                UserName = userName,
                Email = email,
                Endpoint = context.Request.Path,
                Method = context.Request.Method,
                IpAddress = context.Connection.RemoteIpAddress?.ToString(),
                Timestamp = DateTime.UtcNow
            };

            await journalRepository.AddAsync(entry);
        }
    }
}
