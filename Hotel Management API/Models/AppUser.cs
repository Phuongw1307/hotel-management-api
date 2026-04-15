namespace Hotel_Management_API.Models
{
    public class AppUser
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public bool IsActive { get; set; } = true;

        public int TokenVersion { get; set; } = 0;
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }

    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Staff = "Staff";
        public const string User = "User";
    }
}
