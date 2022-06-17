
namespace TicketPal.Domain.Models.Param
{
    public class ConcertSearchParam
    {
        public string Type { get; set; }
        public bool Newest { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string ArtistName { get; set; }
        public string TourName { get; set; }
        public string PerformerId { get; set; }
    }
}