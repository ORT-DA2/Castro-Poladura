using System.Text.Json.Serialization;
using TicketPal.Domain.Models.Response;

namespace TicketPal.Domain.Models.Request
{
    public class AddTicketRequest
    {
        [JsonIgnore]
        public int EventId { get; set; }
        [JsonIgnore]
        public bool UserLogged { get; set; }
        //public TicketBuyer NewUser { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public int LoggedUserId { get; set; }
    }
}
