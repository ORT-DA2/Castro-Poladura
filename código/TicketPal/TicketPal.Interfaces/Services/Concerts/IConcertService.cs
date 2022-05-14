using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Models.Request;
using TicketPal.Domain.Models.Response;

namespace TicketPal.Interfaces.Services.Concerts
{
    public interface IConcertService
    {
        OperationResult AddConcert(AddConcertRequest model);
        OperationResult UpdateConcert(UpdateConcertRequest model);
        OperationResult DeleteConcert(int id);
        IEnumerable<Concert> GetConcerts(EventType type, bool newest, string startDate, string endDate, string artistName);
        Concert GetConcert(int id);
    }
}
