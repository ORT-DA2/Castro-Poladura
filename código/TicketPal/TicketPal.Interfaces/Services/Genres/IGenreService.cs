using System.Collections.Generic;
using TicketPal.Domain.Models.Request;
using TicketPal.Domain.Models.Response;

namespace TicketPal.Interfaces.Services.Genres
{
    public interface IGenreService
    {
        OperationResult AddGenre(AddGenreRequest model);
        OperationResult UpdateGenre(UpdateGenreRequest model);
        OperationResult DeleteGenre(int id);
        IEnumerable<Genre> GetGenres();
        Genre GetGenre(int id);
    }
}
