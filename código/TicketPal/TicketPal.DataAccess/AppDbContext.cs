using Microsoft.EntityFrameworkCore;
using TicketPal.Domain.Entity;
using DbData = TicketPal.DataAccess.Data.SeedData;

namespace TicketPal.DataAccess
{

    public class AppDbContext : DbContext
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

            builder.Entity<UserEntity>()
                .HasIndex(u => u.Email)
                .IsUnique(true);

            builder.Entity<TicketEntity>()
                .HasOne(t => t.Event);

            //Default values for BD
            builder.Entity<UserEntity>()
                .HasData(DbData.Users);

        }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<GenreEntity> Genres { get; set; }
        public DbSet<PerformerEntity> Performers { get; set; }
        public DbSet<ConcertEntity> Concerts { get; set; }
        public DbSet<TicketEntity> Tickets { get; set; }

    }
}