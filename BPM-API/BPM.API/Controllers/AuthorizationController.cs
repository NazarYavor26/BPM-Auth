using Microsoft.AspNetCore.Mvc;
using BPM.BLL.Models;

namespace BPM.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorizationController : ControllerBase
{
    public AuthorizationController()
    {
        
    }
    
    [HttpPost]
    public IActionResult SignIn([FromForm]UserSignInModel userSignIn)
    {
        return Ok();
    }
}