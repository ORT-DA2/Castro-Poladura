using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Exceptions;
using TicketPal.Interfaces.Repository;

namespace TicketPal.DataAccess.Repository
{
    public class GenreRepository : GenericRepository<GenreEntity>
    {
        public GenreRepository(DbContext context) : base(context)
        {
        }

        public override void Add(GenreEntity element)
        {
            if (Exists(element.Id))
            {
                throw new RepositoryException("The genre you are trying to add already exists");
            }

            dbContext.Set<GenreEntity>().Add(element);
            element.CreatedAt = DateTime.Now;
            dbContext.SaveChanges();
        }

        public override void Update(GenreEntity element)
        {
            var found = dbContext.Set<GenreEntity>().FirstOrDefault(g => g.Id == element.Id);

            if (found == null)
            {
                throw new RepositoryException(string.Format("Couldn't find item to update with id: {0} doesn't exist", element.Id));
            }

            found.GenreName = (element.GenreName == null ? found.GenreName : element.GenreName);
            found.UpdatedAt = DateTime.Now;

            dbContext.SaveChanges();
            dbContext.Entry(found).State = EntityState.Modified;
        }
    }
}
