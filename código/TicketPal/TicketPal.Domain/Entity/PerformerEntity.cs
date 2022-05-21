using System.Collections.Generic;

namespace TicketPal.Domain.Entity
{
    public class PerformerEntity : BaseEntity
    {
        public string PerformerType { get; set; }
        public UserEntity UserInfo {get; set;}
        public string StartYear { get; set; }
        public GenreEntity Genre { get; set; }
        public List<ConcertEntity> Concerts { get; set; }
    }
}
