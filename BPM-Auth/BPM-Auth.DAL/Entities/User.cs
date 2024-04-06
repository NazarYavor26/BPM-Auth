using System.ComponentModel.DataAnnotations;

namespace BPM_Auth.DAL.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Email { get; set; }

        public string? Password { get; set; }

        [Required]
        public Enums.Rore RoleId { get; set; }

        public virtual Role Role { get; set; }

        [Required]
        public Enums.Status StatusId { get; set; }

        public virtual Status Status { get; set; }

        public UserProfile Profile { get; set; }
    }
}
