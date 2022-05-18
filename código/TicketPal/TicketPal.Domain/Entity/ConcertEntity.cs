using System.Collections.Generic;

namespace TicketPal.Domain.Entity
{
    public class ConcertEntity : EventEntity
    {
        public string TourName { get; set; }
        public IEnumerable<PerformerEntity> Artists { get; set; }
    }
}
