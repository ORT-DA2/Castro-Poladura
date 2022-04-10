using System;

namespace TicketPal.DataAccess
{
    public class BaseEntity
    {
        public int IdBaseEntity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
