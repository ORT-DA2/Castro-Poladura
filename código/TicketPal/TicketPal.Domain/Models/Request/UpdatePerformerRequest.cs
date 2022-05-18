using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Models.Response;

namespace TicketPal.Domain.Models.Request
{
    public class UpdatePerformerRequest
    {
        public int Id { get; set; }
        [Required]
        public string PerformerType { get; set; }
        [Required]
        public User UserInfo { get; set; }
        [Required]
        public string StartYear { get; set; }
        [Required]
        public int Genre { get; set; }
        [Required]
        public IEnumerable<Performer> Artists { get; set; }
    }
}
