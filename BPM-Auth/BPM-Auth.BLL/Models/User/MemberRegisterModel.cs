using BPM_Auth.DAL.Enums;

namespace BPM_Auth.BLL.Models.User;

public class MemberRegisterModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Position { get; set; }
    public string Email { get; set; }
    public Role Role { get; set; }
    public string Password { get; set; }
    public Guid TeamId { get; set; }
    public Guid SupervisorId { get; set; }
}