using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Models.Request;
using TicketPal.Domain.Models.Response.Error;
using TicketPal.Interfaces.Services.Performers;
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
        [AuthenticationFilter(Constants.ROLE_ADMIN)]
        public async Task<IActionResult> AddPerformer([FromBody] AddPerformerRequest request)
        {
            var result = await performerService.AddPerformer(request);

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
        [AuthenticationFilter(Constants.ROLE_ADMIN)]
        public async Task<IActionResult> UpdatePerformer([FromRoute] int id, [FromBody] UpdatePerformerRequest request)
        {
            request.Id = id;
            request.UserId = id;
            var result = await performerService.UpdatePerformer(request);

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
        public IActionResult DeletePerformer([FromRoute] int id)
        {
            var result = performerService.DeletePerformer(id);

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
        public async Task<IActionResult> GetPerformer([FromRoute] int id)
        {
            return Ok(await performerService.GetPerformer(id));
        }

        [HttpGet]
        [AuthenticationFilter(Constants.ROLE_ADMIN)]
        public async Task<IActionResult> GetPerformers()
        {
            return Ok(await performerService.GetPerformers());
        }

    }
}