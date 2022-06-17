using System;
using TicketPal.Domain.Constants;

namespace TicketPal.Domain.Entity
{
    public class TicketEntity : BaseEntity
    {
        public UserEntity Buyer { get; set; }
        public string Status { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Code { get; set; }
        public EventEntity Event { get; set; }

    }
}
