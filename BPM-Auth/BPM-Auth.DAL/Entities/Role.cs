using System.ComponentModel.DataAnnotations;

namespace BPM_Auth.DAL.Entities
{
    public class Role
    {
        public Enums.Rore Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
