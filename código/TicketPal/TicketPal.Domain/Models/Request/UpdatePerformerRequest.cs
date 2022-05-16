using System.ComponentModel.DataAnnotations;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Entity;

namespace TicketPal.Domain.Models.Request
{
    public class UpdatePerformerRequest
    {
        public int Id { get; set; }
        [Required]
        public string PerformerType { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string StartYear { get; set; }
        [Required]
        public int Genre { get; set; }
        [Required]
        public string Artists { get; set; }
    }
}
