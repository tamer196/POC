using POC.Application.Journal.DTOs;

namespace POC.Application.Journal.Interfaces
{
    public interface IJournalRepository
    {
        Task AddAsync(JournalEntryDto entry);

        Task<List<JournalEntryDto>> SearchAsync(
            Guid? userId,
            string? userName,
            string? email,
            string? endpoint,
            DateTime? from,
            DateTime? to);
    }
}
