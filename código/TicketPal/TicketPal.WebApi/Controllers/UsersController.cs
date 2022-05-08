using Microsoft.AspNetCore.Mvc;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Models.Request;
using TicketPal.Domain.Models.Response.Error;
using TicketPal.Interfaces.Factory;
using TicketPal.Interfaces.Services.Users;
using TicketPal.WebApi.Constants;
using TicketPal.WebApi.Filters.Auth;

namespace TicketPal.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("login")]
        public IActionResult Authenticate([FromBody] AuthenticationRequest request)
        {
            var result = userService.Login(request);
            return Ok(result);
        }

        [HttpPost]
        [TypeFilter(typeof(AuthenticationFilter),
            Arguments = new string[] { Roles.Spectator, Roles.Admin })]
        public IActionResult Register([FromBody] SignUpRequest request)
        {
            var result = userService.SignUp(request);
            if (result.ResultCode == ResultCode.FAIL)
            {
                return BadRequest(new BadRequestError(result.Message));
            }
            else
            {
                return Ok(result);
            }
        }


    }
}