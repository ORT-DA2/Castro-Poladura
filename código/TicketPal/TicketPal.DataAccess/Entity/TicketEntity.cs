using System;
using TicketPal.Domain.Constants;

namespace TicketPal.DataAccess.Entity
{
    public class TicketEntity : BaseEntity
    {
        public User Buyer { get; set; }
        public TicketStatus Status { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string ShowName { get; set; }
        public PerformerEntity Artist { get; set; }

    }
}
