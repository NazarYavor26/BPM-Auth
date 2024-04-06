using Microsoft.AspNetCore.Mvc;
using BPM.BLL.Models;
using BPM.BLL.Services;

namespace BPM.API.Controllers;

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
        var token = _authService.Login(userLoginModel);
        return Ok(token);
    }
}