using System;
using System.Globalization;
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
        [AuthFilter(Constants.ROLE_ADMIN)]
        public IActionResult AddConcert([FromBody] AddConcertRequest request)
        {
            var result = eventService.AddConcert(request);

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
        [AuthFilter(Constants.ROLE_ADMIN)]
        public IActionResult UpdateConcert([FromRoute] int id, [FromBody] UpdateConcertRequest request)
        {
            request.Id = id;
            var result = eventService.UpdateConcert(request);

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
        public IActionResult DeleteConcert([FromRoute] int id)
        {
            var result = eventService.DeleteConcert(id);

            if (result.ResultCode == Constants.CODE_FAIL)
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
                       "dd/M/yyyy",
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None,
                       out dtEnd);
            DateTime dtStart;
            var parseEndDate = DateTime.TryParseExact(endDate,
                       "dd/M/yyyy",
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None,
                       out dtStart);

            if (String.IsNullOrEmpty(startDate) || !parseStartDate)
            {
                return BadRequest(new BadRequestError("Start date not in: dd/M/yyyy"));
            }
            if (String.IsNullOrEmpty(endDate) || !parseEndDate)
            {
                return BadRequest(new BadRequestError("End date not in: dd/M/yyyy"));
            }

            return Ok(eventService.GetConcerts(type,newest,startDate,endDate,performerName));
        }


    }
}