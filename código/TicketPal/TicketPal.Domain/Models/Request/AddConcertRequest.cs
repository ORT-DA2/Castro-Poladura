using System;
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
        public CurrencyType CurrencyType { get; set; }
        [Required]
        public EventType EventType { get; set; }
        [Required]
        public string TourName { get; set; }
        [Required]
        public PerformerEntity Artist { get; set; }
    }
}
