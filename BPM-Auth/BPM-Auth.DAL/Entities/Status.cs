using System.ComponentModel.DataAnnotations;

namespace BPM_Auth.DAL.Entities
{
    public class Status
    {
        public Enums.Status Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
