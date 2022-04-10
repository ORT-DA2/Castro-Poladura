using System;

namespace TicketPal.Domain.Entity
{
    public abstract class EventEntity : BaseEntity
    {
        public int IdEvent { get; set; }
        public DateTime EventDate { get; set; }
        public int AvailableTickets { get; set; }
        public decimal TicketPrice { get; set; }
    }
}
