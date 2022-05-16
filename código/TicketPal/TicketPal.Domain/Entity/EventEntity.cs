﻿using System;

namespace TicketPal.Domain.Entity
{
    public abstract class EventEntity : BaseEntity
    {
        public DateTime Date { get; set; }
        public int AvailableTickets { get; set; }
        public decimal TicketPrice { get; set; }
        public string CurrencyType { get; set; }
        public string EventType { get; set; }
    }
}
