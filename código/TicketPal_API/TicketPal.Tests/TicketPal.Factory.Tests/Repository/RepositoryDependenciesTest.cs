using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicketPal.DataAccess.Repository;
using TicketPal.Domain.Entity;

namespace TicketPal.Factory.Tests.Repository
{
    [TestClass]
    public class RepositoryDependenciesTest : BaseTestFactoryConfig
    {
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