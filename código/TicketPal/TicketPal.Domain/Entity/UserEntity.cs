using System.ComponentModel.DataAnnotations;

namespace TicketPal.Domain.Entity
{
    public class UserEntity : BaseEntity
    {
        [Required]
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public bool ActiveAccount { get; set; }
    }
}