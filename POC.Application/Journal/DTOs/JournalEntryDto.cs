namespace POC.Application.Journal.DTOs
{
    public class JournalEntryDto
    {
        public Guid? UserId { get; set; }

        public string? UserName { get; set; }
        public string? Email { get; set; }

        public string Endpoint { get; set; } = default!;

        public string Method { get; set; } = default!;

        public string IpAddress { get; set; } = default!;

        public DateTime Timestamp { get; set; }
    }
}
