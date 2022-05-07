using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TicketPal.BusinessLogic.Settings.Api;
using TicketPal.Interfaces.Factory;

namespace TicketPal.Factory.Tests
{
    [TestClass]
    public class BaseTestFactoryConfig
    {
        protected IServiceFactory factory;
        protected IServiceCollection services;
        protected Mock<IConfiguration> mockConfig;
        protected Mock<IMapper> mockMapper;
        protected string testConnectionString = "SomeFakeConnectionString";

        [TestInitialize]
        public void SetUp()
        {
            // Mocks inits Configuration
            this.mockConfig = new Mock<IConfiguration>();
            this.mockMapper = new Mock<IMapper>();

            Mock<IConfigurationSection> configurationSectionStub = new Mock<IConfigurationSection>();
            configurationSectionStub.Setup(x => x[It.IsAny<string>()]).Returns(testConnectionString);
            this.mockConfig = new Mock<IConfiguration>();
            this.mockConfig.Setup(x => x.GetSection(It.IsAny<string>())).Returns(configurationSectionStub.Object);

            // Services
            this.services = new ServiceCollection();

            // User Service options configs
            this.services.AddSingleton<IConfiguration>(c => this.mockConfig.Object);
            this.services.AddSingleton<IOptions<IAppSettings>>(c =>
                Options.Create(new AppSettings { JwtSecret = It.IsAny<string>() }));
            this.services.AddOptions();
            // Service collection mock setup
            this.services.AddSingleton<IServiceCollection>(s => this.services);
            this.services.AddSingleton<IMapper>(s => this.mockMapper.Object);

            // Init Factory
            this.factory = new ServiceFactory(
                this.services,
                this.mockConfig.Object
                );

            this.services.AddSingleton<IServiceFactory>(s => this.factory);

            this.factory.AddDbContextService("SomeFakeConnectionString");
            this.factory.RegisterRepositories();
            this.factory.RegisterServices();
            this.factory.BuildServices();

        }
    }
}