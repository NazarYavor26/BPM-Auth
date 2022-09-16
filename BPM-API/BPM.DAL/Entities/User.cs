using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM.DAL.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Email { get; set; }

        [Required]
        public Enums.Rore RoleId { get; set; }

        [Required]
        public Enums.Status StatusId { get; set; }

        public UserProfile Profile { get; set; }
    }
}
