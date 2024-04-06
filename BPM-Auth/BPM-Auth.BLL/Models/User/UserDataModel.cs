using BPM_Auth.DAL.Enums;

namespace BPM_Auth.BLL.Models.User
{
    public class UserDataModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public byte[] PasswordSalt { get; set; }

        public byte[] PasswordHash { get; set; }

        public string RefreshToken { get; set; } = string.Empty;

        public DateTime TokenCreated { get; set; }

        public DateTime TokenExpires { get; set; }

        public Rore RoleId { get; set; }

        public Status StatusId { get; set; }

        public Level LevelId { get; set; }
    }
}
