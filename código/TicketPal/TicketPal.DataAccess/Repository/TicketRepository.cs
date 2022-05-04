using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Exceptions;
using TicketPal.Interfaces.Repository;

namespace TicketPal.DataAccess.Repository
{
    public class TicketRepository : GenericRepository<TicketEntity>
    {
        public TicketRepository(AppDbContext context) : base(context)
        {
        }
            
        public override void Add(TicketEntity element)
        {
            if (Exists(element.Id))
            {
                throw new RepositoryException("The ticket you are trying to add already exists");
            }

            dbContext.Set<TicketEntity>().Add(element);
            element.CreatedAt = DateTime.Now;
            dbContext.SaveChanges();
        }

        public override void Update(TicketEntity element)
        {
            var found = dbContext.Set<TicketEntity>().FirstOrDefault(t => t.Id == element.Id);

            if (found == null)
            {
                throw new RepositoryException(string.Format("Couldn't find item to update with id: {0} doesn't exist", element.Id));
            }

            found.Buyer = (element.Buyer == null ? found.Buyer : element.Buyer);
            found.Event = (element.Event == null ? found.Event : element.Event);
            found.PurchaseDate = (element.PurchaseDate == null ? found.PurchaseDate : element.PurchaseDate);
            found.ShowName = (element.ShowName == null ? found.ShowName : element.ShowName);
            found.Status = (element.Status == null ? found.Status : element.Status);
            found.UpdatedAt = DateTime.Now;

            dbContext.SaveChanges();
            dbContext.Entry(found).State = EntityState.Modified;
        }
    }
}
