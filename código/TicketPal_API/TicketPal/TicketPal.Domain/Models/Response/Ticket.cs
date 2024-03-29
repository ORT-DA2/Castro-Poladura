﻿using System;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Entity;

namespace TicketPal.Domain.Models.Response
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public String PurchaseDate { get; set; }
        public string Code { get; set; }
        public Concert Event { get; set; }
        public User Buyer { get; set; }
    }
}
