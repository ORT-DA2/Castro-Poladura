﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Entity;

namespace TicketPal.Domain.Models.Request
{
    public class AddConcertRequest
    {
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int AvailableTickets { get; set; }
        [Required]
        public decimal TicketPrice { get; set; }
        [Required]
        public string CurrencyType { get; set; }
        [Required]
        public string EventType { get; set; }
        [Required]
        public string TourName { get; set; }
        [Required]
        public IEnumerable<int> ArtistsIds { get; set; }
    }
}
