using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Models.Response;

namespace TicketPal.Domain.Models.Request
{
    public class AddPerformerRequest
    {
        [Required]
        [Range(typeof(string), Constants.Constants.PERFORMER_TYPE_BAND, Constants.Constants.PERFORMER_TYPE_SOLO_ARTIST,
        ErrorMessage = "Value for {0} must be any of the following: {1} or {2}")]
        public string PerformerType { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string StartYear { get; set; }
        [Required]
        public int Genre { get; set; }
        [Required]
        public IEnumerable<int> ConcertIds { get; set; }
    }
}
