using System;
using System.Collections.Generic;
using TicketPal.Domain.Constants;

namespace TicketPal.Domain.Entity
{
    public class PerformerEntity : BaseEntity
    {

        public PerformerType PerformerType { get; set; }
        public string Name { get; set; }
        public string StartYear { get; set; }
        public GenreEntity Genre { get; set; }
        public List<string> Artists { get; set; }
    }
}
