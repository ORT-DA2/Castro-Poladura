using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketPal.Domain.Models.Request;
using TicketPal.Domain.Models.Response;

namespace TicketPal.Interfaces.Services.Tickets
{
    public interface ITicketService
    {
        OperationResult AddTicket(AddTicketRequest model);
        OperationResult UpdateTicket(UpdateTicketRequest model);
        OperationResult DeleteTicket(int id);
        IEnumerable<Ticket> GetTickets();
        Ticket GetTicket(int id);
    }
}
