using BPM_Auth.DAL.Enums;

<<<<<<<< HEAD:BPM-Auth/BPM-Auth.BLL/Models/UserDataModel.cs
namespace BPM.BLL.Models
========
namespace BPM_Auth.BLL.Models.User
>>>>>>>> develop:BPM-Auth/BPM-Auth.BLL/Models/User/UserDataModel.cs
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
