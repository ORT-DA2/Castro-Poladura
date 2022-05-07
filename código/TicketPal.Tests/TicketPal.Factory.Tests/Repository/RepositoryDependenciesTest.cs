using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicketPal.DataAccess.Repository;
using TicketPal.Domain.Entity;
using TicketPal.Interfaces.Repository;

namespace TicketPal.Factory.Tests.Repository
{
    [TestClass]
    public class RepositoryDependenciesTest : BaseTestFactoryConfig
    {
        private ServiceFactory factory;
        private ServiceProvider serviceProvider;

        [TestInitialize]
        public void Init()
        {
            // Factory
            this.factory = new ServiceFactory(
                this.services,
                this.mockConfig.Object
                );

            this.factory.AddDbContextService("SomeFakeConnectionString");
            this.factory.RegisterRepositories();

            this.serviceProvider = services.BuildServiceProvider();
        }

        [TestMethod]
        public void UserRepositoryDependencyCheck()
        {
            var repository = factory.GetRepository(typeof(UserEntity));

            Assert.IsNotNull(repository);
            Assert.IsTrue(repository.GetType() == typeof(UserRepository));
        }

        [TestMethod]
        public void ConcertRepositoryDependencyCheck()
        {
            var repository = factory.GetRepository(typeof(ConcertEntity));

            Assert.IsNotNull(repository);
            Assert.IsTrue(repository.GetType() == typeof(ConcertRepository));
        }

        [TestMethod]
        public void GenreRepositoryDependencyCheck()
        {
            var repository = factory.GetRepository(typeof(GenreEntity));

            Assert.IsNotNull(repository);
            Assert.IsTrue(repository.GetType() == typeof(GenreRepository));
        }

        [TestMethod]
        public void PerformerRepositoryDependencyCheck()
        {
            var repository = factory.GetRepository(typeof(PerformerEntity));

            Assert.IsNotNull(repository);
            Assert.IsTrue(repository.GetType() == typeof(PerformerRepository));
        }

        [TestMethod]
        public void TicketRepositoryDependencyCheck()
        {
            var repository = factory.GetRepository(typeof(TicketEntity));

            Assert.IsNotNull(repository);
            Assert.IsTrue(repository.GetType() == typeof(TicketRepository));
        }
    }
}