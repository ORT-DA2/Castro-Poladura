using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Entity;

namespace TicketPal.Domain.Models.Request
{
    public class UpdateConcertRequest
    {
        [JsonIgnore]
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public decimal TicketPrice { get; set; }
        [Required]
        public string CurrencyType { get; set; }
        [Required]
        public string EventType { get; set; }
        [Required]
        public string TourName { get; set; }
    }
}
