using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TicketPal.Domain.Models.Request
{
    public class UpdateUserRequest
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool ActiveAccount { get; set; }

    }
}
