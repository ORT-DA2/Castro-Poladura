using TicketPal.Domain.Models.Response;

namespace TicketPal.Domain.Models.Request
{
    public class AddTicketRequest
    {
        public int Event { get; set; }
        public User User { get; set; }

    }
}
