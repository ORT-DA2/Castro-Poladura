using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicketPal.Interfaces.Services.Users;

namespace TicketPal.Factory.Tests.Services
{
    [TestClass]
    public class ServiceDependenciesTest: BaseTestFactoryConfig
    {
        private ServiceFactory factory;
        private ServiceProvider serviceProvider;

        [TestInitialize]
        public void Init()
        {
            this.factory = new ServiceFactory(
                this.services,
                this.mockConfig.Object
                );
            
            this.factory.AddDbContextService(testConnectionString);
            this.factory.RegisterRepositories();
            this.factory.RegisterServices();

            this.serviceProvider = services.BuildServiceProvider();
        }

        [TestMethod]
        public void UserServiceDependencyCheck()
        {
            var service = serviceProvider.GetService<IUserService>();
            Assert.IsNotNull(service);
        }
        
    }
}