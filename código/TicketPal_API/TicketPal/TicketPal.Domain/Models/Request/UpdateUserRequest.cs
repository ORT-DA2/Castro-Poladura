using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TicketPal.Domain.Models.Request
{
    public class UpdateUserRequest
    {
        [JsonIgnore]
        public int Id { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(20)]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public bool ActiveAccount { get; set; }

    }
}
