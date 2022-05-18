using System.ComponentModel.DataAnnotations;

namespace TicketPal.Domain.Models.Request
{
    public class SignUpRequest
    {
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(20)]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }
    }
}