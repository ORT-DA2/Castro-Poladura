using System.ComponentModel.DataAnnotations;

namespace TicketPal.Domain.Models.Request
{
    public class TicketBuyer
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
    }
}