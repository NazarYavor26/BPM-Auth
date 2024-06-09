using BPM_Auth.DAL.Enums;

namespace BPM_Auth.DAL.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }

        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }

        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }
    }
}
