using Microsoft.AspNetCore.Mvc;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Models.Request;
using TicketPal.Domain.Models.Response.Error;
using TicketPal.Interfaces.Services.Performers;
using TicketPal.WebApi.Constants;
using TicketPal.WebApi.Filters.Auth;

namespace TicketPal.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PerformersController : ControllerBase
    {
        IPerformerService performerService;

        public PerformersController(IPerformerService service)
        {
            this.performerService = service;
        }

        [HttpPost]
        [AuthenticationFilter(Roles.Admin)]
        public IActionResult AddPerformer([FromBody] AddPerformerRequest request)
        {
            var result = performerService.AddPerformer(request);

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
        [AuthenticationFilter(Roles.Admin)]
        public IActionResult UpdatePerformer([FromRoute]int id, [FromBody]UpdatePerformerRequest request)
        {
            request.Id = id;
            var result = performerService.UpdatePerformer(request);

            if (result.ResultCode == ResultCode.FAIL)
            {
                return BadRequest(new BadRequestError(result.Message));
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpDelete("id")]
        [AuthenticationFilter(Roles.Admin)]
        public IActionResult DeletePerformer([FromRoute]int id)
        {
            var result = performerService.DeletePerformer(id);

            if (result.ResultCode == ResultCode.FAIL)
            {
                return BadRequest(new BadRequestError(result.Message));
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpGet("id")]
        public IActionResult GetPerformer([FromRoute]int id)
        {
            return Ok(performerService.GetPerformer(id));
        }

        [HttpGet]
        public IActionResult GetPerformers()
        {
            return Ok(performerService.GetPerformers());
        }

    }
}