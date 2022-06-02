using System.Collections.Generic;
using System.Threading.Tasks;
using TicketPal.Domain.Models.Request;
using TicketPal.Domain.Models.Response;

namespace TicketPal.Interfaces.Services.Genres
{
    public interface IGenreService
    {
        Task<OperationResult> AddGenre(AddGenreRequest model);
        OperationResult UpdateGenre(UpdateGenreRequest model);
        OperationResult DeleteGenre(int id);
        Task<List<Genre>> GetGenres();
        Task<Genre> GetGenre(int id);
    }
}
