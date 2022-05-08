using System;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Entity;

namespace TicketPal.Domain.Models.Response
{
    public class Concert
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int AvailableTickets { get; set; }
        public decimal TicketPrice { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public EventType EventType { get; set; }
        public string TourName { get; set; }
        public PerformerEntity Artist { get; set; }
    }
}
