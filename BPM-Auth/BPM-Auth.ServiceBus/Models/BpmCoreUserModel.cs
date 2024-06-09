using BPM_Auth.DAL.Enums;

namespace BPM_Auth.ServiceBus.Models
{
    public class BpmCoreUserModel
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public Guid? TeamId { get; set; }
        public Guid? SupervisorId { get; set; }
    }
}
