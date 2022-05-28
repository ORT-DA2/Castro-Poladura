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
        IEnumerable<Concert> GetConcerts(string type, bool newest, string startDate, string endDate, string artistName);
        Concert GetConcert(int id);
    }
}
