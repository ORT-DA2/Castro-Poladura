using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicketPal.BusinessLogic.Services;
using TicketPal.BusinessLogic.Services.Users;
using TicketPal.Interfaces.Factory;
using TicketPal.Interfaces.Services.Jwt;
using TicketPal.Interfaces.Services.Users;

namespace TicketPal.Factory.Tests.Services
{
    [TestClass]
    public class ServiceDependenciesTest : BaseTestFactoryConfig
    {

        [TestMethod]
        public void UserServiceDependencyCheck()
        {
            var service = factory.GetService(typeof(IUserService));

            Assert.IsNotNull(service);
            Assert.IsTrue(service.GetType() == typeof(UserService));
        }

        [TestMethod]
        public void JwtServiceDependencyCheck()
        {
            var service = factory.GetService(typeof(IJwtService));

            Assert.IsNotNull(service);
            Assert.IsTrue(service.GetType() == typeof(JwtService));
        }

    }
}