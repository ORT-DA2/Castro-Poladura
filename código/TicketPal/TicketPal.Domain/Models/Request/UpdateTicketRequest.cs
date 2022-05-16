using System.ComponentModel.DataAnnotations;
using TicketPal.Domain.Constants;

namespace TicketPal.Domain.Models.Request
{
    public class UpdateTicketRequest
    {
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Status { get; set; }
    }
}
