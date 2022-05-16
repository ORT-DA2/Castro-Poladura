using TicketPal.Domain.Constants;
using TicketPal.Domain.Entity;

namespace TicketPal.Domain.Models.Response
{
    public class Performer
    {
        public int Id { get; set; }
        public string PerformerType { get; set; }
        public string Name { get; set; }
        public string StartYear { get; set; }
        public Genre Genre { get; set; }
        public string Artists { get; set; }
    }
}
