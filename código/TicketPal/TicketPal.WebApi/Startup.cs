
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TicketPal.BusinessLogic.Services.Settings;
using TicketPal.Factory;
using TicketPal.Interfaces.Factory;
using TicketPal.WebApi.Filters.Auth;
using TicketPal.WebApi.Filters.Model;

namespace TicketPal.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Settings
            var section = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(section);
            // Filters
            services.AddScoped<AuthFilter>();
            services.AddMvcCore(options =>
            {
                options.Filters.Add(typeof(ModelValidateFilter));
            });
            //Factory
            IServiceFactory factory = new ServiceFactory(
                services,
                Configuration
            );
            factory.AddDbContextService(Configuration.GetConnectionString("TicketPal_SQL_EXPRESS"));
            factory.RegisterRepositories();
            factory.RegisterServices();
            factory.BuildServices();

            services.AddControllers();

            services.Configure<ApiBehaviorOptions>(opt =>
            {
                opt.SuppressModelStateInvalidFilter = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
