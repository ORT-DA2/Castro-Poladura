using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Models.Param;
using TicketPal.Domain.Models.Request;
using TicketPal.Domain.Models.Response.Error;
using TicketPal.Interfaces.Services.Concerts;
using TicketPal.WebApi.Filters.Auth;

namespace TicketPal.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        IConcertService eventService;

        public EventsController(IConcertService service)
        {
            this.eventService = service;
        }

        [HttpPost]
        [AuthenticationFilter(Constants.ROLE_ADMIN)]
        public async Task<IActionResult> AddConcert([FromBody] AddConcertRequest request)
        {
            var result = await eventService.AddConcert(request);

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
        public async Task<IActionResult> UpdateConcert([FromRoute] int id, [FromBody] UpdateConcertRequest request)
        {
            request.Id = id;
            var result = await eventService.UpdateConcert(request);

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
        public async Task<IActionResult> DeleteConcert([FromRoute] int id)
        {
            var result = await eventService.DeleteConcert(id);

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
        public async Task<IActionResult> GetConcert([FromRoute] int id)
        {
            return Ok(await eventService.GetConcert(id));
        }

        [HttpGet]
        public async Task<IActionResult> GetConcerts([FromQuery] ConcertSearchParam param)
        {
            DateTime dtEnd;
            var parseStartDate = DateTime.TryParseExact(param.StartDate,
                "dd/M/yyyy HH:mm",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out dtEnd);
            DateTime dtStart;
            var parseEndDate = DateTime.TryParseExact(param.EndDate,
                "dd/M/yyyy HH:mm",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out dtStart);

            if (!String.IsNullOrEmpty(param.StartDate) && !parseStartDate)
            {
                return BadRequest(new BadRequestError("Start date not in: dd/M/yyyy HH:mm"));
            }

            if (!String.IsNullOrEmpty(param.EndDate) && !parseEndDate)
            {
                return BadRequest(new BadRequestError("End date not in: dd/M/yyyy HH:mm"));
            }

            return Ok(await eventService.GetConcerts(param));
        }

        [HttpGet("performer")]
        public async Task<IActionResult> GetConcertsByPerformerId([FromQuery] ConcertSearchParam param)
        {
            return Ok(await eventService.GetConcertsByPerformerId(param));
        }

    }
}