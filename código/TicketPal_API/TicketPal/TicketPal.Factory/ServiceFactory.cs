using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicketPal.BusinessLogic.Mapper;
using TicketPal.BusinessLogic.Services;
using TicketPal.BusinessLogic.Services.Users;
using TicketPal.BusinessLogic.Services.Concerts;
using TicketPal.BusinessLogic.Services.Genres;
using TicketPal.BusinessLogic.Services.Performers;
using TicketPal.BusinessLogic.Services.Tickets;
using TicketPal.BusinessLogic.Utils.TicketCodes;
using TicketPal.DataAccess;
using TicketPal.DataAccess.Repository;
using TicketPal.Domain.Entity;
using TicketPal.Interfaces.Factory;
using TicketPal.Interfaces.Repository;
using TicketPal.Interfaces.Services.Jwt;
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
                (options => options.UseSqlServer(connectionString),
                ServiceLifetime.Transient);
        }

        public void RegisterRepositories()
        {
            services.AddTransient(typeof(IGenericRepository<UserEntity>), typeof(UserRepository));
            services.AddTransient(typeof(IGenericRepository<ConcertEntity>), typeof(ConcertRepository));
            services.AddTransient(typeof(IGenericRepository<GenreEntity>), typeof(GenreRepository));
            services.AddTransient(typeof(IGenericRepository<PerformerEntity>), typeof(PerformerRepository));
            services.AddTransient(typeof(IGenericRepository<TicketEntity>), typeof(TicketRepository));
        }

        public void RegisterServices()
        {
            LoadMapperServiceConfig();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IConcertService, ConcertService>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<IPerformerService, PerformerService>();
            services.AddScoped<ITicketCode, TicketCode>();
            services.AddScoped<ITicketService, TicketService>();
        }

        public void BuildServices()
        {
            services.AddSingleton<IServiceFactory>(s => this);
            this.serviceProvider = services.BuildServiceProvider();
        }

        private void LoadMapperServiceConfig()
        {
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