using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Models.Request;
using TicketPal.Domain.Models.Response.Error;
using TicketPal.Interfaces.Services.Tickets;
using TicketPal.Interfaces.Services.Users;
using TicketPal.WebApi.Constants;
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
        [AuthFilter(Roles.Seller+","+Roles.Spectator+","+Roles.Admin)]
        public IActionResult AddTicket([FromBody] AddTicketRequest request)
        {
            var result = ticketService.AddTicket(request);

            if (result.ResultCode == ResultCode.FAIL)
            {
                return BadRequest(new BadRequestError(result.Message));
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpPut("{id}")]
        [AuthFilter(Roles.Supervisor+","+Roles.Admin)]
        public IActionResult UpdateTicket([FromRoute]int id, [FromBody] UpdateTicketRequest request)
        {
            request.Id = id;
            var result = ticketService.UpdateTicket(request);

            if (result.ResultCode == ResultCode.FAIL)
            {
                return BadRequest(new BadRequestError(result.Message));
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpDelete("{id}")]
        [AuthFilter(Roles.Admin)]
        public IActionResult DeleteTicket([FromRoute]int id)
        {
            var result = ticketService.DeleteTicket(id);

            if (result.ResultCode == ResultCode.FAIL)
            {
                return BadRequest(new BadRequestError(result.Message));
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpGet("{id}")]
        [AuthFilter(Roles.Admin)]
        public IActionResult GetTicket([FromRoute]int id)
        {
            return Ok(ticketService.GetTicket(id));
        }

        [HttpGet]
        [AuthFilter(Roles.Admin+","+Roles.Spectator)]
        public IActionResult GetTickets()
        {
            this.userService = this.HttpContext
                .RequestServices
                .GetService(typeof(IUserService)) as IUserService;
            var token = HttpContext.Request.Headers["Authorization"]
                .FirstOrDefault()?.Split(" ").Last();
            var authenticatedUser = userService.RetrieveUserFromToken(token);
            
            if(authenticatedUser.Role.Equals(Roles.Admin)) 
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