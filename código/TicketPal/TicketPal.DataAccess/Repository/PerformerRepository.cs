using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Exceptions;
using TicketPal.Interfaces.Repository;

namespace TicketPal.DataAccess.Repository
{
    public class PerformerRepository : GenericRepository<PerformerEntity>
    {
        public PerformerRepository(AppDbContext context) : base(context)
        {
        }

        public override void Add(PerformerEntity element)
        {
            if (Exists(element.Id))
            {
                throw new RepositoryException("The performer you are trying to add already exists");
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

            found.Name = (element.Name == null ? found.Name : element.Name);
            found.PerformerType = element.PerformerType;
            found.StartYear = (element.StartYear == null ? found.StartYear : element.StartYear);
            found.Artists = (element.Artists == null ? found.Artists : element.Artists);
            found.Genre = (element.Genre == null ? found.Genre : element.Genre);
            found.UpdatedAt = DateTime.Now;

            dbContext.SaveChanges();
            dbContext.Entry(found).State = EntityState.Modified;
        }
    }
}
