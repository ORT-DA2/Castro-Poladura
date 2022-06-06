using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TicketPal.Domain.Constants;
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
        public IActionResult GetConcert([FromRoute] int id)
        {
            return Ok(eventService.GetConcert(id));
        }

        [HttpGet]
        public IActionResult GetConcerts(
            [BindRequired][FromQuery] string type,
            [FromQuery(Name = "newest")] bool newest,
            [FromQuery(Name = "startDate")] string startDate,
            [FromQuery(Name = "endDate")] string endDate,
            [FromQuery(Name = "performerName")] string performerName
        )
        {
            DateTime dtEnd;
            var parseStartDate = DateTime.TryParseExact(startDate,
                       "dd/M/yyyy hh:mm",
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None,
                       out dtEnd);
            DateTime dtStart;
            var parseEndDate = DateTime.TryParseExact(endDate,
                       "dd/M/yyyy hh:mm",
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None,
                       out dtStart);

            if (!String.IsNullOrEmpty(startDate) && !parseStartDate)
            {
                return BadRequest(new BadRequestError("Start date not in: dd/M/yyyy hh:mm"));
            }
            if (!String.IsNullOrEmpty(endDate) && !parseEndDate)
            {
                return BadRequest(new BadRequestError("End date not in: dd/M/yyyy hh:mm"));
            }

            return Ok(eventService.GetConcerts(type, newest, startDate, endDate, performerName));
        }


    }
}