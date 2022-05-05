using System.ComponentModel.DataAnnotations;

namespace TicketPal.Domain.Models.Request
{
    public class AuthenticationRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}