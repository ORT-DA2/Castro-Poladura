using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Exceptions;

namespace TicketPal.DataAccess.Repository
{
    public class GenreRepository : GenericRepository<GenreEntity>
    {
        public GenreRepository(AppDbContext context) : base(context)
        {
        }

        public override void Add(GenreEntity element)
        {
            if (Exists(element.Id) || ExistGenreName(element.GenreName))
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

        public bool ExistGenreName (string name)
        {
            return dbContext.Set<GenreEntity>().Any(item => item.GenreName == name);
        }
    }
}
