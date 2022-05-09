using System.Collections.Generic;
using TicketPal.Domain.Models.Request;
using TicketPal.Domain.Models.Response;

namespace TicketPal.Interfaces.Services.Concerts
{
    public interface IConcertService
    {
        OperationResult AddConcert(AddConcertRequest model);
        OperationResult UpdateConcert(UpdateConcertRequest model);
        OperationResult DeleteConcert(int id);
        IEnumerable<Concert> GetConcerts();
        Concert GetConcert(int id);
    }
}
