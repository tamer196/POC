namespace POC.Infrastructure.Mongo
{
    public class MongoSettings
    {
        public string ConnectionString { get; set; } = default!;
        public string Database { get; set; } = default!;
        public string JournalCollection { get; set; } = "Journal";
    }
}
