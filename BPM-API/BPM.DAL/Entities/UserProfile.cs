using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM.DAL.Entities
{
    public class UserProfile
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(150)]
        public string LastName { get; set; }

        [Required]
        public int UserId { get; set; }

        public User User { get; set; }

        [Required]
        public int LevelId { get; set; }

        public virtual Level Level { get; set; }

        [Required]
        public virtual User LastUpdatedBy { get; set; }

        [Required]
        public virtual User CompetenceLeadId { get; set; }
    }
}
