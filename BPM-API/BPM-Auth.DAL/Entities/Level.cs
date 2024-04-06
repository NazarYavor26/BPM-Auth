using System.ComponentModel.DataAnnotations;

namespace BPM_Auth.DAL.Entities
{
    public class Level
    {
        public Enums.Level Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public virtual ICollection<UserProfile> UserProfiles { get; set; }
    }
}
