using Microsoft.EntityFrameworkCore;
using TicketPal.Domain.Entity;

namespace TicketPal.DataAccess
{
    public interface IAppDbContext
    {
        DbSet<UserEntity> Users { get; set; }
        DbSet<GenreEntity> Genres { get; set; }
        DbSet<PerformerEntity> Performers { get; set; }
        DbSet<ConcertEntity> Concerts { get; set; }
        DbSet<TicketEntity> Tickets { get; set; }
    }
}