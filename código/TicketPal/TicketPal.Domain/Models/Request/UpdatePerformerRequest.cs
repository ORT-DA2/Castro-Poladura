using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Models.Response;

namespace TicketPal.Domain.Models.Request
{
    public class UpdatePerformerRequest
    {
        [JsonIgnore]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string PerformerType { get; set; }
        [Required]
        public string StartYear { get; set; }
        [Required]
        public int GenreId { get; set; }
        [Required]
        public IEnumerable<int> ArtistsIds { get; set; }
    }
}
