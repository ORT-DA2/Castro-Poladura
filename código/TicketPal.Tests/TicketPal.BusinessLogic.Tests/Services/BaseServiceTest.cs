using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TicketPal.BusinessLogic.Mapper;
using TicketPal.BusinessLogic.Settings.Api;
using TicketPal.Domain.Entity;
using TicketPal.Interfaces.Factory;
using TicketPal.Interfaces.Repository;
using TicketPal.Interfaces.Services.Jwt;
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
        // Services
        protected IUserService userService;
        protected IJwtService jwtService;
        // Configs
        protected IMapper mapper;
        protected string jwtTestSecret;
        protected string userPassword;
        protected IOptions<AppSettings> testAppSettings;


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

            // User repository test settings
            this.userPassword = "somePassword";
            this.jwtTestSecret = "23jrb783v29fwfvfg2874gf286fce8";
            this.testAppSettings = Options.Create(new AppSettings { JwtSecret = jwtTestSecret });
        }

    }
}