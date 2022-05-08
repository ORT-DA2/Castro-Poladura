using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TicketPal.BusinessLogic.Mapper;
using TicketPal.BusinessLogic.Services.Settings;
using TicketPal.Domain.Entity;
using TicketPal.Interfaces.Factory;
using TicketPal.Interfaces.Repository;
using TicketPal.Interfaces.Services;
using TicketPal.Interfaces.Services.Jwt;
using TicketPal.Interfaces.Services.Settings;
using TicketPal.Interfaces.Services.Users;

namespace TicketPal.BusinessLogic.Tests.Services
{
    [TestClass]
    public abstract class BaseServiceTest
    {
        // Mocks
        protected Mock<DbContext> mockDbContext;
        protected Mock<IServiceFactory> factoryMock;
        protected Mock<IGenericRepository<UserEntity>> mockUserRepo;
        protected Mock<IJwtService> mockJwtService;
        protected Mock<IAppSettings> mockAppSettings;
        // Services
        protected IUserService userService;
        protected IJwtService jwtService;
        protected IAppSettings appSettings;
        protected IOptions<IAppSettings> options;
        // Configs
        protected IMapper mapper;
        protected string jwtTestSecret;
        protected string userPassword;

        [TestInitialize]
        public void SetUp()
        {
            // Context mock
            this.mockDbContext = new Mock<DbContext>(MockBehavior.Loose);
            // Mapper mock
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapping>());
            this.mapper = mapperConfig.CreateMapper();

            // Init Mocks
            this.factoryMock = new Mock<IServiceFactory>();
            this.mockUserRepo = new Mock<IGenericRepository<UserEntity>>();
            this.mockJwtService = new Mock<IJwtService>(MockBehavior.Default);
            this.mockAppSettings = new Mock<IAppSettings>(MockBehavior.Default);

            // User repository test settings
            this.userPassword = "somePassword";
            this.jwtTestSecret = "23jrb783v29fwfvfg2874gf286fce8";

            this.mockAppSettings.Setup(s => s.JwtSecret).Returns("fakeSecret");
            this.options = Options.Create(mockAppSettings.Object);
        }

    }
}