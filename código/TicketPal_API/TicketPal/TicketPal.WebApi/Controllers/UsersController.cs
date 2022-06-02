using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Models.Request;
using TicketPal.Domain.Models.Response.Error;
using TicketPal.Interfaces.Services.Users;
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
        public async Task<IActionResult> Authenticate([FromBody] AuthenticationRequest request)
        {
            var user = await userService.Login(request);
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                var forbidden = new ForbiddenError("Email or password incorrect.");
                return new ObjectResult(forbidden)
                {
                    StatusCode = forbidden.StatusCode
                };
            }
        }

        [HttpPost]
        [AuthenticationFilter(Constants.ROLE_ADMIN)]
        public IActionResult Register([FromBody] SignUpRequest request)
        {
            var result = userService.SignUp(request);

            if (result.ResultCode == Constants.CODE_FAIL)
            {
                return BadRequest(new BadRequestError(result.Message));
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpGet("{id}")]
        [AuthenticationFilter(Constants.ROLE_ADMIN + "," + Constants.ROLE_SPECTATOR)]
        public async Task<IActionResult> GetUserAccount([FromRoute] int id)
        {
            var token = HttpContext.Request.Headers["Authorization"]
                .FirstOrDefault()?.Split(" ").Last();
            var authenticatedUser = await userService.RetrieveUserFromToken(token);

            if (authenticatedUser.Role != Constants.ROLE_ADMIN &&
                authenticatedUser.Id != id)
            {
                var forbidden = new ForbiddenError("No required authorization to access resource.");
                return new ObjectResult(forbidden)
                {
                    StatusCode = forbidden.StatusCode
                };
            }

            return Ok(userService.GetUser(id));
        }

        [HttpGet]
        [AuthenticationFilter(Constants.ROLE_ADMIN)]
        public async Task<IActionResult> GetUserAccounts([FromQuery(Name = "role")] string role)
        {
            return Ok(await userService.GetUsers(role));
        }

        [HttpPut("{id}")]
        [AuthenticationFilter(Constants.ROLE_ADMIN + "," + Constants.ROLE_SPECTATOR)]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateUserRequest request)
        {
            var token = HttpContext.Request.Headers["Authorization"]
                .FirstOrDefault()?.Split(" ").Last();
            var authenticatedUser = await userService.RetrieveUserFromToken(token);

            if (!authenticatedUser.Role.Equals(Constants.ROLE_ADMIN) &&
            !authenticatedUser.Role.Equals(request.Role) &&
            authenticatedUser.Id != id)
            {
                var forbidden = new ForbiddenError("No required authorization to access resource.");
                return new ObjectResult(forbidden)
                {
                    StatusCode = forbidden.StatusCode
                };
            }
            else
            {
                request.Id = id;
                var result = userService.UpdateUser(request, authenticatedUser.Role);

                if (result.ResultCode == Constants.CODE_FAIL)
                {
                    return BadRequest(new BadRequestError(result.Message));
                }
                else
                {
                    return Ok(result);
                }
            }
        }

        [HttpDelete("{id}")]
        [AuthenticationFilter(Constants.ROLE_ADMIN)]
        public IActionResult DeleteAccount([FromRoute] int id)
        {
            var result = userService.DeleteUser(id);

            if (result.ResultCode == Constants.CODE_FAIL)
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