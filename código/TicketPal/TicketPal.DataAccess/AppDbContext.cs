
using Microsoft.EntityFrameworkCore;
using TicketPal.Domain.Entity;

namespace TicketPal.DataAccess
{

    public class AppDbContext : DbContext, IAppDbContext
    {
        protected AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //BD mapping rules


            //Default values for BD

        }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<GenreEntity> Genres { get; set; }
        public DbSet<PerformerEntity> Performers { get; set; }

    }
}