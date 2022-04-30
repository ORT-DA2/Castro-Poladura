using TicketPal.Domain.Entity;
using TicketPal.Interfaces.Repository;

namespace TicketPal.DataAccess.Repository
{
    public interface IUnitOfWork
    {
        IGenericRepository<UserEntity> Users { get; }
        IGenericRepository<GenreEntity> Genres { get; }
        IGenericRepository<PerformerEntity> Performers { get; }
    }
}