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
            this.factory = new ServiceFactory(this.services);
            
            this.factory.AddDbContextService("SomeFakeConnectionString");
            this.factory.RegisterRepositories();
            this.factory.RegisterServices();
        }

        [TestMethod]
        public void UserServiceDependencyCheck()
        {
            var service = serviceProvider.GetService<IUserService>();
            Assert.IsNotNull(service);
        }
        
    }
}