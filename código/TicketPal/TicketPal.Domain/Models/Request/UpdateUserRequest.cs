using System.ComponentModel.DataAnnotations;

namespace TicketPal.Domain.Models.Request
{
    public class UpdateUserRequest
    {
        public int Id { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        [MinLength(10)]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Role { get; set; }

    }
}
