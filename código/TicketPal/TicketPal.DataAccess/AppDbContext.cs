
using Microsoft.EntityFrameworkCore;
using TicketPal.Domain.Entity;

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


            //Default values for BD

        }
        public DbSet<UserEntity> Accounts { get; set; }

    }
}