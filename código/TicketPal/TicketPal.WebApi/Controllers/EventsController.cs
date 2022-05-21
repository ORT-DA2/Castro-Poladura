using System;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Models.Request;
using TicketPal.Domain.Models.Response.Error;
using TicketPal.Interfaces.Services.Concerts;
using TicketPal.WebApi.Constants;
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
        [AuthenticationFilter(Roles.Admin)]
        public IActionResult AddConcert([FromBody] AddConcertRequest request)
        {
            var result = eventService.AddConcert(request);

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
        public IActionResult UpdateConcert([FromRoute] int id, [FromBody] UpdateConcertRequest request)
        {
            request.Id = id;
            var result = eventService.UpdateConcert(request);

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
        [AuthenticationFilter(Roles.Admin)]
        public IActionResult DeleteConcert([FromRoute] int id)
        {
            var result = eventService.DeleteConcert(id);

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
        public IActionResult GetConcert([FromRoute] int id)
        {
            return Ok(eventService.GetConcert(id));
        }

        // Need to refactor to use in service in future release
        [HttpGet]
        public IActionResult GetConcerts(
            [BindRequired][FromQuery] int type,
            [FromQuery(Name = "newest")] bool newest,
            [FromQuery(Name = "startDate")] string startDate,
            [FromQuery(Name = "endDate")] string endDate,
            [FromQuery(Name = "performerName")] string performerName
        )
        {
            DateTime dtEnd;
            var parseStartDate = DateTime.TryParseExact(startDate,
                       "dd/M/yyyy",
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None,
                       out dtEnd);
            DateTime dtStart;
            var parseEndDate = DateTime.TryParseExact(startDate,
                       "dd/M/yyyy",
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None,
                       out dtStart);
            if (String.IsNullOrEmpty(startDate) && !parseStartDate)
            {
                return BadRequest(new BadRequestError("Start date not in: dd/M/yyyy"));
            }
            if (String.IsNullOrEmpty(endDate) && !parseEndDate)
            {
                return BadRequest(new BadRequestError("End date not in: dd/M/yyyy"));
            }

            if (String.IsNullOrEmpty(startDate)
                && String.IsNullOrEmpty(endDate)
                && String.IsNullOrEmpty(performerName)
                )
            {
                return Ok(eventService.GetConcerts(
                    e => ((int)e.EventType) == type
                    , newest
                    ));
            }
            else if (!String.IsNullOrEmpty(startDate)
            && String.IsNullOrEmpty(endDate)
            && String.IsNullOrEmpty(performerName)
            )
            {
                return Ok(eventService.GetConcerts(
                    e => ((int)e.EventType) == type
                    && e.Date >= dtStart
                    , newest
                    ));
            }
            else if (String.IsNullOrEmpty(startDate)
            && !String.IsNullOrEmpty(endDate)
            && String.IsNullOrEmpty(performerName)
            )
            {
                return Ok(eventService.GetConcerts(
                    e => ((int)e.EventType) == type
                    && e.Date <= dtEnd
                    , newest
                    ));
            }
            else if (!String.IsNullOrEmpty(startDate)
            && !String.IsNullOrEmpty(endDate)
            && String.IsNullOrEmpty(performerName))
            {
                return Ok(eventService.GetConcerts(
                    e => ((int)e.EventType) == type
                    && (e.Date >= dtStart && e.Date <= dtEnd)
                    , newest
                    ));
            }
            else
            {
                return Ok(eventService.GetConcerts(
                    e => ((int)e.EventType) == type
                    && (e.Date >= dtStart && e.Date <= dtEnd)
                    && e.Artist.Name.Equals(performerName)
                    , newest
                    ));
            }

        }


    }
}