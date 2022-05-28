using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Models.Request;
using TicketPal.Domain.Models.Response.Error;
using TicketPal.Interfaces.Services.Tickets;
using TicketPal.Interfaces.Services.Users;
using TicketPal.WebApi.Filters.Auth;

namespace TicketPal.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private ITicketService ticketService;
        private IUserService userService;

        public TicketsController(ITicketService service)
        {
            this.ticketService = service;
        }

        [HttpPost("purchase/{eventId}")]
        [AuthenticationFilter(Constants.ROLE_SELLER + "," + Constants.ROLE_SPECTATOR + "," + Constants.ROLE_ADMIN)]
        public IActionResult AddTicket([FromRoute] int eventId, [FromBody] AddTicketRequest request)
        {
            this.userService = this.HttpContext
                .RequestServices
                .GetService(typeof(IUserService)) as IUserService;
            request.EventId = eventId;
            var token = HttpContext.Request.Headers["Authorization"]
                .FirstOrDefault()?.Split(" ").Last();
            var authenticatedUser = userService.RetrieveUserFromToken(token);

            if (authenticatedUser != null && authenticatedUser.Role.Equals(Constants.ROLE_SPECTATOR))
            {
                request.UserLogged = true;
                request.LoggedUserId = authenticatedUser.Id;
            }

            var result = ticketService.AddTicket(request);

            if (result.ResultCode == Constants.CODE_FAIL)
            {
                return BadRequest(new BadRequestError(result.Message));
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpPut("{id}")]
        [AuthenticationFilter(Constants.ROLES_SUPERVISOR + "," + Constants.ROLE_ADMIN)]
        public IActionResult UpdateTicket([FromRoute] int id, [FromBody] UpdateTicketRequest request)
        {
            request.Id = id;
            var result = ticketService.UpdateTicket(request);

            if (result.ResultCode == Constants.CODE_FAIL)
            {
                return BadRequest(new BadRequestError(result.Message));
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpDelete("{id}")]
        [AuthenticationFilter(Constants.ROLE_ADMIN)]
        public IActionResult DeleteTicket([FromRoute] int id)
        {
            var result = ticketService.DeleteTicket(id);

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
        [AuthenticationFilter(Constants.ROLE_ADMIN)]
        public IActionResult GetTicket([FromRoute] int id)
        {
            return Ok(ticketService.GetTicket(id));
        }

        [HttpGet]
        [AuthenticationFilter(Constants.ROLE_ADMIN + "," + Constants.ROLE_SPECTATOR)]
        public IActionResult GetTickets()
        {
            this.userService = this.HttpContext
                .RequestServices
                .GetService(typeof(IUserService)) as IUserService;
            var token = HttpContext.Request.Headers["Authorization"]
                .FirstOrDefault()?.Split(" ").Last();
            var authenticatedUser = userService.RetrieveUserFromToken(token);

            if (authenticatedUser.Role.Equals(Constants.ROLE_ADMIN))
            {
                return Ok(ticketService.GetTickets());
            }
            else
            {
                return Ok(ticketService.GetUserTickets(authenticatedUser.Id));
            }
        }

    }
}