namespace TicketPal.Domain.Entity
{
    public class ConcertEntity : EventEntity
    {
        public string TourName { get; set; }
        public PerformerEntity Artist { get; set; }
    }
}
