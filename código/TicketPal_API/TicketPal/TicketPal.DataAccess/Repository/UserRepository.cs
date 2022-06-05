using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Exceptions;

namespace TicketPal.DataAccess.Repository
{
    public class UserRepository : GenericRepository<UserEntity>
    {
        public UserRepository(DbContext context) : base(context) { }
        public override void Update(UserEntity element)
        {
            var found = dbContext.Set<UserEntity>().FirstOrDefault(u => u.Id == element.Id);

            if (found == null)
            {
                throw new RepositoryException(string.Format("Couldn't find item to update with id: {0} doesn't exist", element.Id));
            }

            found.Role = (element.Role == null ? found.Role : element.Role);
            found.Firstname = (element.Firstname == null ? found.Firstname : element.Firstname);
            found.Lastname = (element.Lastname == null ? found.Lastname : element.Lastname);
            found.Email = (element.Email == null ? found.Email : element.Email);
            found.Password = (element.Password == null ? found.Password : element.Password);
            found.UpdatedAt = DateTime.Now;

            dbContext.SaveChanges();
            dbContext.Entry(found).State = EntityState.Modified;
        }

        public override async Task Add(UserEntity user)
        {
            var emails = await GetAll(u => u.Email.Equals(user.Email));

            if (emails.Any())
            {
                throw new RepositoryException("Email already registered");
            }

            user.CreatedAt = DateTime.Now;
            dbContext.Set<UserEntity>().Add(user);
            dbContext.SaveChanges();
        }

        public async override Task<UserEntity> Get(int id)
        {
            return await dbContext.Set<UserEntity>()
             .Include(u => u.Performer)
             .ThenInclude(p => p.Members)
             .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async override Task<UserEntity> Get(Expression<Func<UserEntity, bool>> predicate)
        {
            return await dbContext.Set<UserEntity>()
            .Include(u => u.Performer)
            .ThenInclude(p => p.Members)
            .FirstOrDefaultAsync(predicate);
        }

        public async override Task<List<UserEntity>> GetAll()
        {
            return await dbContext.Set<UserEntity>()
            .Include(u => u.Performer)
            .ThenInclude(p => p.Members)
            .ToListAsync();
        }

        public async override Task<List<UserEntity>> GetAll(Expression<Func<UserEntity, bool>> predicate)
        {
            return await dbContext.Set<UserEntity>()
            .Include(u => u.Performer)
            .ThenInclude(p => p.Members)
            .Where(predicate)
            .ToListAsync();
        }
    }
}