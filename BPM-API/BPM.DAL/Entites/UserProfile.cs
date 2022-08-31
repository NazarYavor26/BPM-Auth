using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM.DAL.Entites
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
        public int LastUpdatedBy { get; set; }

        public int CompetenceLeadId { get; set; }

        public User User { get; set; }


    }
}
