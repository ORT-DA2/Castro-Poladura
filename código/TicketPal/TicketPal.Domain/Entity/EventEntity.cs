using System;
using TicketPal.Domain.Constants;

namespace TicketPal.Domain.Entity
{
    public abstract class EventEntity : BaseEntity
    {
        public DateTime Date { get; set; }
        public int AvailableTickets { get; set; }
        public decimal TicketPrice { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public EventType EventType { get; set; }
    }
}
