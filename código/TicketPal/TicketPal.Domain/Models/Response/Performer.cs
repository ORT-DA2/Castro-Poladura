using System.Collections.Generic;
using TicketPal.Domain.Entity;

namespace TicketPal.Domain.Models.Response
{
    public class Performer
    {
        public int Id { get; set; }
        public User UserInfo { get; set; }
        public string PerformerType { get; set; }
        public string StartYear { get; set; }
        public Genre Genre { get; set; }
        public IEnumerable<Performer> Members { get; set; }
    }
}
