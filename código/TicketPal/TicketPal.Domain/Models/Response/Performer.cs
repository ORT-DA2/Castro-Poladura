using System.Collections.Generic;

namespace TicketPal.Domain.Models.Response
{
    public class Performer
    {
        public int Id { get; set; }
        public User UserInfo { get; set; }
        public string PerformerType { get; set; }
        public string StartYear { get; set; }
        public Genre Genre { get; set; }
        public IEnumerable<Concert> Concerts { get; set; }
    }
}
