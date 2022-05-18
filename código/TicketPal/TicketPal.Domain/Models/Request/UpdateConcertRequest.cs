using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Entity;

namespace TicketPal.Domain.Models.Request
{
    public class UpdateConcertRequest
    {
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public decimal TicketPrice { get; set; }
        [Required]
        [Range(typeof(string), Constants.Constants.CURRENCY_URUGUAYAN_PESO, Constants.Constants.CURRENCY_US_DOLLARS,
        ErrorMessage = "Value for {0} must be any of the following: {1} or {2}")]
        public string CurrencyType { get; set; }
        [Required]
        [Range(typeof(string), Constants.Constants.EVENT_CONCERT_TYPE,Constants.Constants.EVENT_CONCERT_TYPE,
        ErrorMessage = "Value for {0} must be any of the following: {0}")]
        public string EventType { get; set; }
        [Required]
        public string TourName { get; set; }
        [Required]
        public IEnumerable<int> Artists { get; set; }
    }
}
