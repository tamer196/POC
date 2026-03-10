using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

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
    }
}
