using BPM_Auth.BLL.Models.User;
using BPM_Auth.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace BPM_Auth.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public ActionResult Register(UserRegisterModel userRegisterModel)
    {
        var registerResult = _authService.Register(userRegisterModel);
        return Ok(registerResult);
    }


    [HttpPost("login")]
    public ActionResult Login(UserLoginModel userLoginModel)
    {
        var token = _authService.Login(userLoginModel, Response);
        return Ok(token);
    }

    [HttpPost("refresh-token")]
    public ActionResult RefreshToken()
    {
        var token = _authService.RefreshToken(Request, Response);
        return Ok(token);
    }
}