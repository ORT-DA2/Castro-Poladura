using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TicketPal.DataAccess;
using TicketPal.DataAccess.Repository;
using TicketPal.Domain.Entity;
using TicketPal.Interfaces.Repository;

namespace TicketPal.Factory
{
    public class ServiceFactory
    {
        private readonly IServiceCollection services;

        public ServiceFactory(IServiceCollection services)
		{
			this.services = services;
		}

        public void AddDbContextService(string connectionString)
		{
			services.AddDbContext<DbContext, AppDbContext>
                (options => options.UseSqlServer(connectionString));
		}

        public void RegisterRepositories()
        {
            services.AddScoped(typeof(IGenericRepository<UserEntity>),typeof(UserRepository));
        }
    }
}