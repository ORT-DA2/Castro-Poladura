using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Models.Request;
using TicketPal.Domain.Models.Response.Error;
using TicketPal.Interfaces.Services.Genres;
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
        [AuthenticationFilter(Constants.ROLE_ADMIN)]
        public async Task<IActionResult> AddGenre(AddGenreRequest request)
        {
            var result = await genreService.AddGenre(request);

            if (result.ResultCode == Constants.CODE_FAIL)
            {
                return BadRequest(new BadRequestError(result.Message));
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [AuthenticationFilter(Constants.ROLE_ADMIN)]
        public IActionResult DeleteGenre([FromRoute] int id)
        {
            var result = genreService.DeleteGenre(id);

            if (result.ResultCode == Constants.CODE_FAIL)
            {
                return BadRequest(new BadRequestError(result.Message));
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGenre([FromRoute] int id)
        {
            return Ok(await genreService.GetGenre(id));
        }

        [HttpGet]
        public IActionResult GetGenres()
        {
            return Ok(genreService.GetGenres());
        }

        [HttpPut("{id}")]
        [AuthenticationFilter(Constants.ROLE_ADMIN)]
        public IActionResult UpdateGenre([FromRoute] int id, UpdateGenreRequest request)
        {
            request.Id = id;
            var result = genreService.UpdateGenre(request);

            if (result.ResultCode == Constants.CODE_FAIL)
            {
                return BadRequest(new BadRequestError(result.Message));
            }

            return Ok(result);
        }


    }
}