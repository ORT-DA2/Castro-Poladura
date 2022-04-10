using System.ComponentModel.DataAnnotations;

namespace TicketPal.Domain.Entity
{
    public class UserEntity : BaseEntity
    {
        [Required]
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}