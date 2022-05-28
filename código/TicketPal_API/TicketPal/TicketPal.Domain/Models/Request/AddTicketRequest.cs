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
        public TicketBuyer NewUser { get; set; }
        [JsonIgnore]
        public int LoggedUserId { get; set; }
    }
}
