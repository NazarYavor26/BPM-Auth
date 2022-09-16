using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM.DAL.Entities
{
    public class Level
    {
        public Enums.Level Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
