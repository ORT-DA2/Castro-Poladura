using Microsoft.AspNetCore.Mvc;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Models.Request;
using TicketPal.Domain.Models.Response.Error;
using TicketPal.Interfaces.Services.Genres;
using TicketPal.WebApi.Constants;
using TicketPal.WebApi.Filters.Auth;

namespace TicketPal.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenresController : ControllerBase
    {
        private IGenreService genreService;

        public GenresController(IGenreService genreService)
        {
            this.genreService = genreService;
        }

        [HttpPost]
        [AuthFilter(Roles.Admin)]
        public IActionResult AddGenre(AddGenreRequest request)
        {
            var result = genreService.AddGenre(request);

            if(result.ResultCode == ResultCode.FAIL)
            {
                return BadRequest(new BadRequestError(result.Message));
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [AuthFilter(Roles.Admin)]
        public IActionResult DeleteGenre([FromRoute]int id)
        {
            var result = genreService.DeleteGenre(id);

            if(result.ResultCode == ResultCode.FAIL)
            {
                return BadRequest(new BadRequestError(result.Message));
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetGenre([FromRoute]int id)
        {
            return Ok(genreService.GetGenre(id));
        }

        [HttpGet]
        public IActionResult GetGenres()
        {
            return Ok(genreService.GetGenres());
        }

        [HttpPut("{id}")]
        [AuthFilter(Roles.Admin)]
        public IActionResult UpdateGenre([FromRoute]int id, UpdateGenreRequest request)
        {
            request.Id = id;
            var result = genreService.UpdateGenre(request);

            if(result.ResultCode == ResultCode.FAIL)
            {
                return BadRequest(new BadRequestError(result.Message));
            }
            
            return Ok(result);
        }


    }
}