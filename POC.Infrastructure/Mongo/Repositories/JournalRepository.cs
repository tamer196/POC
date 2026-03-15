using MongoDB.Driver;
using POC.Application.Journal.DTOs;
using POC.Application.Journal.Interfaces;
using POC.Infrastructure.Mongo.Entities;
using SharpCompress.Common;

namespace POC.Infrastructure.Mongo.Repositories
{
    public class JournalRepository : IJournalRepository
    {
        private readonly MongoDbContext _context;

        public JournalRepository(MongoDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(JournalEntryDto entry)
        {
            await _context.Journal.InsertOneAsync(new JournalEntry
            {
                UserId = entry.UserId,
                UserName = entry.UserName,
                Email = entry.Email,
                Endpoint = entry.Endpoint,
                Method = entry.Method,
                IpAddress = entry.IpAddress,
                Timestamp = entry.Timestamp,
                Entity = entry.Entity,
                EntityId = entry.EntityId,
                OldValues = entry.OldValues,
                NewValues = entry.NewValues
            });
        }

        public async Task<List<JournalEntryDto>> SearchAsync(
            Guid? userId,
            string? userName,
            string? email,
            string? endpoint,
            DateTime? from,
            DateTime? to)
        {
            var filter = Builders<JournalEntry>.Filter.Empty;

            if (userId.HasValue)
                filter &= Builders<JournalEntry>.Filter.Eq(x => x.UserId, userId);

            if (!string.IsNullOrEmpty(userName))
                filter &= Builders<JournalEntry>.Filter.Eq(x => x.UserName, userName);

            if (!string.IsNullOrEmpty(email))
                filter &= Builders<JournalEntry>.Filter.Eq(x => x.Email, email);

            if (!string.IsNullOrEmpty(endpoint))
                filter &= Builders<JournalEntry>.Filter.Eq(x => x.Endpoint, endpoint);

            if (from.HasValue)
                filter &= Builders<JournalEntry>.Filter.Gte(x => x.Timestamp, from);

            if (to.HasValue)
                filter &= Builders<JournalEntry>.Filter.Lte(x => x.Timestamp, to);

            var result = await _context.Journal
                .Find(filter)
                .SortByDescending(x => x.Timestamp)
                .ToListAsync();

            return result.Select(x => new JournalEntryDto
            {
                UserId = x.UserId,
                UserName = x.UserName,
                Email = x.Email,
                Endpoint = x.Endpoint,
                Method = x.Method,
                IpAddress = x.IpAddress,
                Timestamp = x.Timestamp,
                Entity = x.Entity,
                EntityId = x.EntityId,
                OldValues = x.OldValues,
                NewValues = x.NewValues

            }).ToList();
        }
    }
}
