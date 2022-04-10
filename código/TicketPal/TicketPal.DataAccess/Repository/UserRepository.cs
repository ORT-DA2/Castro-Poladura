using System;
using System.Linq;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Exceptions;

namespace TicketPal.DataAccess.Repository
{
    public class UserRepository : GenericRepository<UserEntity>
    {
        public UserRepository(AppDbContext context) : base(context) { }
        public override void Update(UserEntity element)
        {
            throw new System.NotImplementedException();
        }

        public override void Add(UserEntity user)
        {
            var duplicateEmail = GetAll(u => u.Email.Equals(user.Email)).Any();

            if (duplicateEmail)
            {
                throw new RepositoryException("Email already registered");

            }

            user.CreatedAt = DateTime.Now;
            dbContext.Set<UserEntity>().Add(user);
            dbContext.SaveChanges();
        }
    }
}