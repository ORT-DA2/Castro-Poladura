using System;
using TicketPal.Interfaces.Enumerations;

namespace TicketPal.DataAccess
{
    public class TicketEntity
    {
        public int IdTicket { get; set; }
        public User Buyer { get; set; }
        public TicketStatusEnumeration Status { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string ShowName { get; set; }
        public PerformerEntity Artist { get; set; }

    }
}
