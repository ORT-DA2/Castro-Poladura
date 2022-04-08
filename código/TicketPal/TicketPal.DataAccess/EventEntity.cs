using System;

namespace TicketPal.DataAccess
{
    public class EventEntity
    {
        public int IdEvent { get; set; }
        public DateTime EventDate { get; set; }
        public int AvailableTickets { get; set; }
        public decimal TicketPrice { get; set; }
    }
}
