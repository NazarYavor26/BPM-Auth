using System.ComponentModel.DataAnnotations;

namespace BPM.BLL.Models;

public class UserSignInModel
{
    [Required]
    public string Login { get; set; }
    
    [Required]
    public string Password { get; set; }
}