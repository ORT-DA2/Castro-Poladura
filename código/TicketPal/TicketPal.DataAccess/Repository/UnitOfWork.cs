using Microsoft.EntityFrameworkCore;
using TicketPal.Domain.Entity;
using TicketPal.Interfaces.Repository;

namespace TicketPal.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private IGenericRepository<UserEntity> users;
        private AppDbContext dbContext;

        public UnitOfWork(DbContext context)
        {
            this.dbContext = (AppDbContext)context;
        }
        public IGenericRepository<UserEntity> Users
        {
            get
            {
                if (users == null)
                {
                    this.users = new UserRepository(dbContext);
                    return this.users;
                }
                return this.users;
            }
        }
    }
}