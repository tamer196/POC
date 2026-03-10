using Microsoft.Extensions.Options;
using MongoDB.Driver;
using POC.Infrastructure.Mongo.Entities;

namespace POC.Infrastructure.Mongo
{
    public class MongoDbContext
    {
        public IMongoCollection<JournalEntry> Journal { get; }

        public MongoDbContext(IOptions<MongoSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.Database);

            Journal = database.GetCollection<JournalEntry>(settings.Value.JournalCollection);
        }
    }
}
