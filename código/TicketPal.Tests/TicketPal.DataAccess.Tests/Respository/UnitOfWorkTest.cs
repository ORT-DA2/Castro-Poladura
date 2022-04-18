
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
        private Mock<IUnitOfWork<AppDbContext>> uow;
        private AppDbContext dbContext;

        [TestInitialize]
        public void Initialize()
        {
            uow = new Mock<IUnitOfWork<PlatformContext>>();
            uow.Setup(x => x.GenericRepository<UserEntity>())
                .Returns(SetUpUsersRepository());
        }

        [TestMethod]
        public void GetUserRepository()
        {

            Assert.IsNotNull(uow.UserRepository);
        }

        private IGenericRepository<UserEntity> SetUpUsersRepository()
        {
            Mock<IGenericRepository<UserEntity>> mockAccountsRepo
                = new Mock<IGenericRepository<UserEntity>>(MockBehavior.Default);
            return mockAccountsRepo.Object;

        }

    }
}