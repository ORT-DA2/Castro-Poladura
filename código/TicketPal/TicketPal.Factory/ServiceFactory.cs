using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TicketPal.BusinessLogic.Mapper;
using TicketPal.BusinessLogic.Services.Concerts;
using TicketPal.BusinessLogic.Services.Genres;
using TicketPal.BusinessLogic.Services.Performers;
using TicketPal.BusinessLogic.Services.Tickets;
using TicketPal.BusinessLogic.Services.Users;
using TicketPal.BusinessLogic.Settings.Api;
using TicketPal.BusinessLogic.Utils.TicketCodes;
using TicketPal.DataAccess;
using TicketPal.DataAccess.Repository;
using TicketPal.Domain.Entity;
using TicketPal.Interfaces.Factory;
using TicketPal.Interfaces.Repository;
using TicketPal.Interfaces.Services.Concerts;
using TicketPal.Interfaces.Services.Genres;
using TicketPal.Interfaces.Services.Performers;
using TicketPal.Interfaces.Services.Tickets;
using TicketPal.Interfaces.Services.Users;
using TicketPal.Interfaces.Utils.TicketCodes;

namespace TicketPal.Factory
{
    public class ServiceFactory : IServiceFactory
    {
        private ServiceProvider serviceProvider;
        private readonly IServiceCollection services;
        private readonly IConfiguration configuration;

        public ServiceFactory(
            IServiceCollection services,
            IConfiguration configuration
        )
        {
            this.services = services;
            this.configuration = configuration;
        }

        public void AddDbContextService(string connectionString)
        {
            services.AddDbContext<DbContext, AppDbContext>
                (options => options.UseSqlServer(connectionString));
        }

        public void RegisterRepositories()
        {
            services.AddScoped(typeof(IGenericRepository<UserEntity>), typeof(UserRepository));
            services.AddScoped(typeof(IGenericRepository<ConcertEntity>), typeof(ConcertRepository));
            services.AddScoped(typeof(IGenericRepository<GenreEntity>), typeof(GenreRepository));
            services.AddScoped(typeof(IGenericRepository<PerformerEntity>), typeof(PerformerRepository));
            services.AddScoped(typeof(IGenericRepository<TicketEntity>), typeof(TicketRepository));
        }

        public void RegisterServices()
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IConcertService, ConcertService>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<IPerformerService, PerformerService>();
            services.AddScoped<ITicketCode, TicketCode>();
            services.AddScoped<ITicketService, TicketService>();
        }

        public void BuildServices()
        {
            this.serviceProvider = services.BuildServiceProvider();
        }

        private void LoadConfig()
        {
            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

            // Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapping());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        public object GetRepository(Type classType)
        {
            var genericClass = typeof(IGenericRepository<>);
            var constructedClass = genericClass.MakeGenericType(classType);

            return serviceProvider.GetService(constructedClass);
        }

        public object GetService(Type classType)
        {
            return serviceProvider.GetService(classType);
        }
    }
}