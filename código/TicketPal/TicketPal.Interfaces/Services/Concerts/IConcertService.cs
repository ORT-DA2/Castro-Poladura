using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketPal.Domain.Constants;
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
        bool ExistsConcertByTourName(string tourName);
    }
}
