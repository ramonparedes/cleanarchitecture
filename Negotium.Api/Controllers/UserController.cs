using Microsoft.AspNetCore.Mvc;
using Negotium.Data;
using Negotium.Repository;
using Negotium.Service;

namespace Negotium.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("get")]
        public IActionResult Get()
        {
            var users = _userService.Get();

            return Ok(users);
        }


    }
}