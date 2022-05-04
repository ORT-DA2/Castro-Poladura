
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TicketPal.BusinessLogic.Mapper;
using TicketPal.Interfaces.Repository;
using TicketPal.Interfaces.Services.Users;

namespace TicketPal.BusinessLogic.Tests.Services
{
    [TestClass]
    public abstract class BaseServiceTest
    {
        protected Mock<DbContext> mockDbContext;
        protected IMapper mapper;
        // Repository mocks
        protected Mock<IUserRepository> usersMock;
        // Services
        protected IUserService userService;


        [TestInitialize]
        public void SetUp()
        {
            // Context mock
            this.mockDbContext = new Mock<DbContext>(MockBehavior.Loose);
            // Mapper mock
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapping>());
            this.mapper = mapperConfig.CreateMapper();

            // Repositories mock
            this.usersMock = new Mock<IUserRepository>();
        }

    }
}