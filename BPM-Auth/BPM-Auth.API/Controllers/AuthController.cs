using BPM_Auth.BLL.Models;
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

    [HttpPost("register-admin")]
    public ActionResult RegisterAdmin(AdminRegisterModel adminRegisterModel)
    {
        var registerResult = _authService.RegisterAdmin(adminRegisterModel);
        return Ok(registerResult);
    }

    [HttpPost("register-team")]
    public ActionResult RegisterTeam(TeamRegisterModel teamRegisterModel)
    {
        var registerResult = _authService.RegisterTeam(teamRegisterModel);
        return Ok(registerResult);
    }

    [HttpPost("register-member")]
    public ActionResult RegisterMember(UserRegisterModel userRegisterModel)
    {
        var registerResult = _authService.RegisterMember(userRegisterModel);
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