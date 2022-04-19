using TicketPal.Domain.Entity;
using TicketPal.Interfaces.Repository;

namespace TicketPal.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private IGenericRepository<UserEntity> _users;
        private AppDbContext dbContext;
        public IGenericRepository<UserEntity> Users
        {
            get
            {
                if (_users == null)
                {
                    this._users = new UserRepository(dbContext);
                    return this._users;
                }
                return this._users;
            }
        }
    }
}