using System;
using System.Collections.Generic;
using TicketPal.Domain.Constants;

namespace TicketPal.DataAccess.Entity
{
    public class PerformerEntity : BaseEntity
    {

        public PerformerType PerformerType { get; set; }
        public string Name { get; set; }
        public DateTime StartYear { get; set; }
        public GenreEntity Genre { get; set; }
        public List<PerformerEntity> Artists { get; set; } //Probablemente acá, lo que debería hacerse
        //es que, la clase PerformerEntity, sea como la clase EventEntity, y que de ella hereden las clases
        //Banda, Solista, etc.
    }
}
