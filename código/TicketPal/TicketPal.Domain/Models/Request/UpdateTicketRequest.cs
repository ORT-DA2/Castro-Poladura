using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TicketPal.Domain.Constants;

namespace TicketPal.Domain.Models.Request
{
    public class UpdateTicketRequest
    {
        [JsonIgnore]
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Status { get; set; }
    }
}
