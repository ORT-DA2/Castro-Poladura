using System.ComponentModel.DataAnnotations;
using TicketPal.Domain.Constants;

namespace TicketPal.Domain.Models.Request
{
    public class UpdateTicketRequest
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public TicketStatus Status { get; set; }
    }
}
