using Microsoft.EntityFrameworkCore;
using TicketPal.Domain.Entity;

namespace TicketPal.DataAccess
{
    public interface IAppDbContext
    {
        DbSet<UserEntity> Users { get; set; }
        DbSet<GenreEntity> Genres { get; set; }
    }
}