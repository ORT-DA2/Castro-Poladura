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
    public class TicketController : ControllerBase
    {
        private ITicketService ticketService;
        private IUserService userService;

        public TicketController(ITicketService service)
        {
            this.ticketService = service;
        }

        [HttpPost]
        [AuthFilter(Constants.ROLE_SELLER+","+Constants.ROLE_SPECTATOR+","+Constants.ROLE_ADMIN)]
        public IActionResult AddTicket([FromBody] AddTicketRequest request)
        {
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
        [AuthFilter(Constants.ROLES_SUPERVISOR+","+Constants.ROLE_ADMIN)]
        public IActionResult UpdateTicket([FromRoute]int id, [FromBody] UpdateTicketRequest request)
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
        [AuthFilter(Constants.ROLE_ADMIN)]
        public IActionResult DeleteTicket([FromRoute]int id)
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
        [AuthFilter(Constants.ROLE_ADMIN)]
        public IActionResult GetTicket([FromRoute]int id)
        {
            return Ok(ticketService.GetTicket(id));
        }

        [HttpGet]
        [AuthFilter(Constants.ROLE_ADMIN+","+Constants.ROLE_SPECTATOR)]
        public IActionResult GetTickets()
        {
            this.userService = this.HttpContext
                .RequestServices
                .GetService(typeof(IUserService)) as IUserService;
            var token = HttpContext.Request.Headers["Authorization"]
                .FirstOrDefault()?.Split(" ").Last();
            var authenticatedUser = userService.RetrieveUserFromToken(token);
            
            if(authenticatedUser.Role.Equals(Constants.ROLE_ADMIN)) 
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