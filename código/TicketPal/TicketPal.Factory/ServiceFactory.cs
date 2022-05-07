using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TicketPal.BusinessLogic.Mapper;
using TicketPal.BusinessLogic.Services.Users;
using TicketPal.BusinessLogic.Settings.Api;
using TicketPal.DataAccess;
using TicketPal.DataAccess.Repository;
using TicketPal.Domain.Entity;
using TicketPal.Interfaces.Repository;
using TicketPal.Interfaces.Services.Users;

namespace TicketPal.Factory
{
    public class ServiceFactory
    {
        private readonly IServiceCollection services;
        private readonly IConfiguration configuration;

        public ServiceFactory(
            IServiceCollection services,
            IConfiguration configuration
        )
        {
            this.services = services;
            this.configuration = configuration;
            LoadConfig();
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
            var serviceProvider = services.BuildServiceProvider();

            var genericClass = typeof(IGenericRepository<>);
            var constructedClass = genericClass.MakeGenericType(classType);

            var repository = serviceProvider.GetService(constructedClass);
            return repository;
        }
    }
}