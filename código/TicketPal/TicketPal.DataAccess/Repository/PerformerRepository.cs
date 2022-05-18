using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Exceptions;

namespace TicketPal.DataAccess.Repository
{
    public class PerformerRepository : GenericRepository<PerformerEntity>
    {
        public PerformerRepository(DbContext context) : base(context)
        {
        }

        public override void Add(PerformerEntity element)
        {
            if (Exists(element.Id))
            {
                throw new RepositoryException("The performer you are trying to add already exists");
            }
            if(element.Concerts != null)
            {
                foreach(ConcertEntity concert in element.Concerts)
                {
                    if(concert != null && concert.Id != 0)
                    {
                        dbContext.Attach(concert);
                    }
                }
            }

            dbContext.Set<PerformerEntity>().Add(element);
            element.CreatedAt = DateTime.Now;
            dbContext.SaveChanges();
        }

        public override void Update(PerformerEntity element)
        {
            var found = dbContext.Set<PerformerEntity>().FirstOrDefault(p => p.Id == element.Id);

            if (found == null)
            {
                throw new RepositoryException(string.Format("Couldn't find item to update with id: {0} doesn't exist", element.Id));
            }

            found.UserInfo = (element.UserInfo == null ? found.UserInfo : element.UserInfo);
            found.PerformerType = element.PerformerType;
            found.StartYear = (element.StartYear == null ? found.StartYear : element.StartYear);
            found.Concerts = (element.Concerts == null ? found.Concerts : element.Concerts);
            found.Genre = (element.Genre == null ? found.Genre : element.Genre);
            found.UpdatedAt = DateTime.Now;

            dbContext.SaveChanges();
            dbContext.Entry(found).State = EntityState.Modified;
        }

        public override PerformerEntity Get(int id)
        {
            return dbContext.Set<PerformerEntity>()
            .Include(p => p.UserInfo)
            .Include(p => p.Genre)
            .Include(p => p.Concerts)
            .ThenInclude(c => c.Artists)
            .FirstOrDefault(u => u.Id == id);
        }

        public override PerformerEntity Get(Expression<Func<PerformerEntity, bool>> predicate)
        {
            return dbContext.Set<PerformerEntity>()
            .Include(p => p.UserInfo)
            .Include(p => p.Genre)
            .Include(p => p.Concerts)
            .ThenInclude(c => c.Artists)
            .FirstOrDefault(predicate);
        }

        public override IEnumerable<PerformerEntity> GetAll()
        {
            return dbContext.Set<PerformerEntity>()
            .Include(p => p.UserInfo)
            .Include(p => p.Genre)
            .Include(p => p.Concerts)
            .ThenInclude(c => c.Artists)
            .AsEnumerable();
        }

        public override IEnumerable<PerformerEntity> GetAll(Expression<Func<PerformerEntity, bool>> predicate)
        {
            return dbContext.Set<PerformerEntity>()
            .Include(p => p.UserInfo)
            .Include(p => p.Genre)
            .Include(p => p.Concerts)
            .ThenInclude(c => c.Artists)
            .Where(predicate)
            .AsEnumerable();
        }
    }
}
