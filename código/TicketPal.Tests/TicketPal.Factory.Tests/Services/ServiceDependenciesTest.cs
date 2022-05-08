using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicketPal.BusinessLogic.Services.Concerts;
using TicketPal.BusinessLogic.Services.Genres;
using TicketPal.BusinessLogic.Services.Performers;
using TicketPal.BusinessLogic.Services.Tickets;
using TicketPal.BusinessLogic.Services.Users;
using TicketPal.Interfaces.Services.Concerts;
using TicketPal.Interfaces.Services.Genres;
using TicketPal.Interfaces.Services.Performers;
using TicketPal.Interfaces.Services.Tickets;
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
        public void ConcertServiceDependencyCheck()
        {
            var service = factory.GetService(typeof(IConcertService));

            Assert.IsNotNull(service);
            Assert.IsTrue(service.GetType() == typeof(ConcertService));
        }

        [TestMethod]
        public void GenreServiceDependencyCheck()
        {
            var service = factory.GetService(typeof(IGenreService));

            Assert.IsNotNull(service);
            Assert.IsTrue(service.GetType() == typeof(GenreService));
        }

        [TestMethod]
        public void PerformerServiceDependencyCheck()
        {
            var service = factory.GetService(typeof(IPerformerService));

            Assert.IsNotNull(service);
            Assert.IsTrue(service.GetType() == typeof(PerformerService));
        }

        [TestMethod]
        public void TicketServiceDependencyCheck()
        {
            var service = factory.GetService(typeof(ITicketService));

            Assert.IsNotNull(service);
            Assert.IsTrue(service.GetType() == typeof(TicketService));
        }

    }
}