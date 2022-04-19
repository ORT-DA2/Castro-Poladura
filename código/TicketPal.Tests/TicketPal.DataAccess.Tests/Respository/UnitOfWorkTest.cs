
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TicketPal.DataAccess.Repository;
using TicketPal.Domain.Entity;
using TicketPal.Interfaces.Repository;

namespace TicketPal.DataAccess.Tests.Respository
{
    [TestClass]
    public class UnitOfWorkTest
    {
        private Mock<IUnitOfWork> uow;

        [TestInitialize]
        public void Initialize()
        {
            uow = new Mock<IUnitOfWork>();
        }

        [TestMethod]
        public void GetUserRepository()
        {

            uow.Setup(x => x.Users)
                .Returns(SetUpUsersRepository());

            var userRepo = uow.Object.Users;
            Assert.IsNotNull(userRepo);
        }

        private IGenericRepository<UserEntity> SetUpUsersRepository()
        {
            Mock<IGenericRepository<UserEntity>> mockAccountsRepo
                = new Mock<IGenericRepository<UserEntity>>(MockBehavior.Default);
            return mockAccountsRepo.Object;

        }

    }
}