using BPM.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM.BLL.Models
{
    public class UserDataModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public Rore RoleId { get; set; }

        public Status StatusId { get; set; }

        public Level LevelId { get; set; }
    }
}
