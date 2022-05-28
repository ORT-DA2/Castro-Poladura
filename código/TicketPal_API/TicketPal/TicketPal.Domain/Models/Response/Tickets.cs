using System;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Entity;

namespace TicketPal.Domain.Models.Response
{
    public class Tickets
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Code { get; set; }
        public EventEntity Event { get; set; }
    }
}
