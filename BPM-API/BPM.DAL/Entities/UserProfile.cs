using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM.DAL.Entities
{
    internal class UserProfile
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(150)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(150)]
        public string LastName { get; set; }

        public int LevelId { get; set; }

        [Required]
        public DateTime LastUpdated { get; set; }

        [Required]

        public int? CompetenceLeadId { get; set; }

        public User User { get; set; }

        public virtual Level Level { get; set; }

        //public virtual User LastUpdatedBy { get; set; }

        //public virtual User CompetenceLeadId { get; set; }
    }
}
