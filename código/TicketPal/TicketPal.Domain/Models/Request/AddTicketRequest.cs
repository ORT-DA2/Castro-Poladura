using System.ComponentModel.DataAnnotations;

namespace TicketPal.Domain.Models.Request
{
    public class AddTicketRequest
    {
        [Required]
        public string BuyerFirstName { get; set; }
        [Required]
        public string BuyerLastName { get; set; }
        [Required]
        public string BuyerEmail { get; set; }
    }
}
