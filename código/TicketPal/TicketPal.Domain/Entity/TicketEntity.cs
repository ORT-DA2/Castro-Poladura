using System;
using TicketPal.Domain.Constants;

namespace TicketPal.Domain.Entity
{
    public class TicketEntity : BaseEntity
    {
        public UserEntity Buyer { get; set; }
        public TicketStatus Status { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string ShowName { get; set; }
        public EventEntity Event { get; set; }

    }
}
