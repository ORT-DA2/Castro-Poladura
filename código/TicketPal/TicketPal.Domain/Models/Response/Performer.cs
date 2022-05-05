using TicketPal.Domain.Constants;
using TicketPal.Domain.Entity;

namespace TicketPal.Domain.Models.Response
{
    internal class Performer
    {
        public int Id { get; set; }
        public PerformerType PerformerType { get; set; }
        public string Name { get; set; }
        public string StartYear { get; set; }
        public GenreEntity Genre { get; set; }
        public string Artists { get; set; }
    }
}
