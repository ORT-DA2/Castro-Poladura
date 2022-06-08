using System.ComponentModel.DataAnnotations;

namespace TicketPal.Domain.Models.Request
{
    public class AddGenreRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
