namespace POC.API.Features.Users.DTO
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string UserName { get; set; } = default!;

        public string Email { get; set; } = default!;

        public string Role { get; set; } = default!;

        public bool IsActive { get; set; }
    }
}
