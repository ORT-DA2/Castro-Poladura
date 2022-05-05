using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Exceptions;
using TicketPal.Interfaces.Repository;

namespace TicketPal.DataAccess.Repository
{
    public class ConcertRepository : GenericRepository<ConcertEntity>
    {
        public ConcertRepository(AppDbContext context) : base(context)
        {
        }

        public override void Add(ConcertEntity element)
        {
            if (Exists(element.Id))
            {
                throw new RepositoryException("The event you are trying to add already exists");
            }

            dbContext.Set<ConcertEntity>().Add(element);
            element.CreatedAt = DateTime.Now;
            dbContext.SaveChanges();
        }

        public override void Update(ConcertEntity element)
        {
            var found = dbContext.Set<ConcertEntity>().FirstOrDefault(c => c.Id == element.Id);

            if (found == null)
            {
                throw new RepositoryException(string.Format("Couldn't find item to update with id: {0} doesn't exist", element.Id));
            }

            found.Date = (element.Date == null ? found.Date : element.Date);
            found.AvailableTickets = (element.AvailableTickets == null ? found.AvailableTickets : element.AvailableTickets);
            found.TicketPrice = (element.TicketPrice == null ? found.TicketPrice : element.TicketPrice);
            found.CurrencyType = (element.CurrencyType == null ? found.CurrencyType : element.CurrencyType);
            found.EventType = (element.EventType == null ? found.EventType : element.EventType);
            found.UpdatedAt = DateTime.Now;

            dbContext.SaveChanges();
            dbContext.Entry(found).State = EntityState.Modified;
        }
    }
}
