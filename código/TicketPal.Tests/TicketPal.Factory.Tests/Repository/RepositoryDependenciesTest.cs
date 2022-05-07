using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            var repository = serviceProvider.GetService<IGenericRepository<UserEntity>>();
            Assert.IsNotNull(repository);
        }

        [TestMethod]
        public void ConcertRepositoryDependencyCheck()
        {
            var repository = serviceProvider.GetService<IGenericRepository<ConcertEntity>>();
            Assert.IsNotNull(repository);
        }

        [TestMethod]
        public void GenreRepositoryDependencyCheck()
        {
            var repository = serviceProvider.GetService<IGenericRepository<GenreEntity>>();
            Assert.IsNotNull(repository);
        }

        [TestMethod]
        public void PerformerRepositoryDependencyCheck()
        {
            var repository = serviceProvider.GetService<IGenericRepository<PerformerEntity>>();
            Assert.IsNotNull(repository);
        }

        [TestMethod]
        public void TicketRepositoryDependencyCheck()
        {
            var repository = serviceProvider.GetService<IGenericRepository<TicketEntity>>();
            Assert.IsNotNull(repository);
        }
    }
}