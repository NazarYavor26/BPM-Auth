using BPM_Auth.BLL.Models.User;
using BPM_Auth.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace BPM_Auth.API.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("data")]
        public async Task<ActionResult<UserDataModel>> GetUserData()
        {
            return Ok(await Task.FromResult(_userService.GetUserData()));
        }
    }
}
