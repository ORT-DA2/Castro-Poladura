using Microsoft.AspNetCore.Mvc;
using TicketPal.Domain.Constants;
using Microsoft.AspNetCore.Http;
using TicketPal.Domain.Models.Request;
using TicketPal.Domain.Models.Response.Error;
using TicketPal.Interfaces.Services.Tickets;
using TicketPal.WebApi.Filters.Auth;
using Newtonsoft.Json;
using TicketPal.Domain.Models.Response;
using System.Threading.Tasks;

namespace TicketPal.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private ITicketService ticketService;

        public TicketsController(ITicketService service)
        {
            this.ticketService = service;
        }

        [HttpPost("purchase/{eventId}")]
        [AuthenticationFilter(Constants.ROLE_SELLER + "," + Constants.ROLE_SPECTATOR + "," + Constants.ROLE_ADMIN)]
        public async Task<IActionResult> AddTicket([FromRoute] int eventId, [FromBody] AddTicketRequest request = null)
        {
            if (request.NewUser == null)
            {
                request.NewUser = new TicketBuyer();
            }
            request.EventId = eventId;
            var json = HttpContext.Session.GetString("user");
            var authenticatedUser = JsonConvert.DeserializeObject<User>(json);

            if (authenticatedUser != null && authenticatedUser.Role.Equals(Constants.ROLE_SPECTATOR))
            {
                request.UserLogged = true;
                request.LoggedUserId = authenticatedUser.Id;
            }

            var result = await ticketService.AddTicket(request);

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
        public async Task<IActionResult> DeleteTicket([FromRoute] int id)
        {
            var result = await ticketService.DeleteTicket(id);

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
        public async Task<IActionResult> GetTicket([FromRoute] int id)
        {
            return Ok(await ticketService.GetTicket(id));
        }

        [HttpGet("code/{code}")]
        public async Task<IActionResult> GetTicketByCode([FromRoute] string code)
        {
            return Ok(await ticketService.GetTicketByCode(code));
        }

        [HttpGet]
        [AuthenticationFilter(Constants.ROLE_ADMIN + "," + Constants.ROLE_SPECTATOR + "," + Constants.ROLE_SELLER)]
        public async Task<IActionResult> GetTickets()
        {
            var json = HttpContext.Session.GetString("user");
            var authenticatedUser = JsonConvert.DeserializeObject<User>(json);

            if (authenticatedUser.Role.Equals(Constants.ROLE_ADMIN) || authenticatedUser.Role.Equals(Constants.ROLE_SELLER))
            {
                return Ok(await ticketService.GetTickets());
            }
            else
            {
                return Ok(await ticketService.GetUserTickets(authenticatedUser.Id));
            }
        }

    }
}