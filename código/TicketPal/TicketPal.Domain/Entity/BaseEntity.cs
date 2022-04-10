using System;
using System.ComponentModel.DataAnnotations;

namespace TicketPal.Domain.Entity
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
