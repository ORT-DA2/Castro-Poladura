using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TicketPal.Domain.Entity;
using TicketPal.Interfaces.Repository;

namespace TicketPal.BusinessLogic.Tests.UnitOfWork
{
    [TestClass]
    public class UnitOfWorkTest
    {
        private IUnitOfWork unitOfWork;
        

        [TestMethod]
        public void ShouldGetUserRepository()
        {
            var mockContext = new Mock<DbContext>();
            unitOfWork = new UnitOfWork(mockContext.Object);
            var users = unitOfWork.Users;

            Assert.IsNotNull(users);

        }

        private IGenericRepository<UserEntity> SetUpAccountsRepository()
        {
            Mock<IGenericRepository<UserEntity>> mock 
                = new Mock<IGenericRepository<UserEntity>>(MockBehavior.Default);
            return mock.Object;
        }
    }
}