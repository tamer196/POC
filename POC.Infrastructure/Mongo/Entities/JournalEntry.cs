using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Options;

namespace POC.Infrastructure.Mongo.Entities
{
    public class JournalEntry
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = default!;

        [BsonRepresentation(BsonType.String)]
        public Guid? UserId { get; set; }

        public string? UserName { get; set; }

        public string? Email { get; set; }

        public string Endpoint { get; set; } = default!;

        public string Method { get; set; } = default!;

        public string IpAddress { get; set; } = default!;

        public DateTime Timestamp { get; set; }

        public string? Entity { get; set; }

        public string? EntityId { get; set; }

        // Use dictionary serialization options for better type handling
        [BsonDictionaryOptions(DictionaryRepresentation.Document)]
        public Dictionary<string, object>? OldValues { get; set; }

        [BsonDictionaryOptions(DictionaryRepresentation.Document)]
        public Dictionary<string, object>? NewValues { get; set; }
    }
}