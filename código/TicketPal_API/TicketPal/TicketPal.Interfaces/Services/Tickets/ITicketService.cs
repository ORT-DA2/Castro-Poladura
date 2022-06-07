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
        Task<OperationResult> AddTicket(AddTicketRequest model);
        OperationResult UpdateTicket(UpdateTicketRequest model);
        Task<OperationResult> DeleteTicket(int id);
        Task<List<Ticket>> GetUserTickets(int userId);
        Task<List<Ticket>> GetTickets();
        Task<Ticket> GetTicket(int id);
    }
}
