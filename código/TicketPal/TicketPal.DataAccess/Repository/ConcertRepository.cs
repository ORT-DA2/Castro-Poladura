using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Exceptions;

namespace TicketPal.DataAccess.Repository
{
    public class ConcertRepository : GenericRepository<ConcertEntity>
    {
        public ConcertRepository(DbContext context) : base(context)
        {
        }

        public override void Add(ConcertEntity element)
        {
            if (Exists(element.Id))
            {
                throw new RepositoryException("The event you are trying to add already exists");
            }

            if(element.Artists != null)
            {
                foreach (PerformerEntity i in element.Artists)
                {
                    if (i != null && i.Id != 0) 
                    {
                         dbContext.Attach(i);
                    }
                }
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

            found.Date = element.Date;
            found.AvailableTickets = element.AvailableTickets;
            found.TicketPrice = element.TicketPrice;
            found.CurrencyType = element.CurrencyType == null ? found.CurrencyType : element.CurrencyType;
            found.EventType = element.EventType == null ? found.EventType : element.EventType;
            found.UpdatedAt = DateTime.Now;
            found.TourName = element.TourName == null ? found.TourName : element.TourName;

            dbContext.SaveChanges();
            dbContext.Entry(found).State = EntityState.Modified;
        }

        public override ConcertEntity Get(int id)
        {
            return dbContext.Set<ConcertEntity>()
                .Include(c => c.Artists)
                .ThenInclude(a => a.UserInfo)
                .FirstOrDefault(u => u.Id == id);
        }

        public override ConcertEntity Get(Expression<Func<ConcertEntity, bool>> predicate)
        {
            return dbContext.Set<ConcertEntity>()
                .Include(c => c.Artists)
                .ThenInclude(a => a.UserInfo)
                .FirstOrDefault(predicate);
        }

        public override IEnumerable<ConcertEntity> GetAll()
        {
            return dbContext.Set<ConcertEntity>()
                .Include(c => c.Artists)
                .ThenInclude(a => a.UserInfo)
                .AsEnumerable();
        }

        public override IEnumerable<ConcertEntity> GetAll(Expression<Func<ConcertEntity, bool>> predicate)
        {
            return dbContext.Set<ConcertEntity>()
                .Include(c => c.Artists)
                .ThenInclude(a => a.UserInfo)
                .Where(predicate)
                .AsEnumerable();
        }
    }
}
