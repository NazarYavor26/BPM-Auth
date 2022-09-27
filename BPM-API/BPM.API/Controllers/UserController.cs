using BPM.BLL.Models;
using BPM.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace BPM.API.Controllers
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
