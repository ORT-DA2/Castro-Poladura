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
using TicketPal.Interfaces.Services.Jwt;
using TicketPal.Interfaces.Services.Users;

namespace TicketPal.BusinessLogic.Tests.Services
{
    [TestClass]
    public abstract class BaseServiceTests
    {
        // Mocks
        protected Mock<DbContext> mockDbContext;
        protected Mock<IServiceFactory> factoryMock;
        protected Mock<IGenericRepository<UserEntity>> mockUserRepo;
        protected Mock<IJwtService> mockJwtService;
        protected Mock<IGenericRepository<ConcertEntity>> mockConcertRepo;
        protected Mock<IGenericRepository<GenreEntity>> mockGenreRepo;
        protected Mock<IGenericRepository<PerformerEntity>> mockPerformerRepo;
        protected Mock<IGenericRepository<TicketEntity>> mockTicketRepo;
        // Services
        protected IUserService userService;
        protected IJwtService jwtService;

        protected IOptions<AppSettings> options;
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
            this.mockConcertRepo = new Mock<IGenericRepository<ConcertEntity>>();
            this.mockGenreRepo = new Mock<IGenericRepository<GenreEntity>>();
            this.mockPerformerRepo = new Mock<IGenericRepository<PerformerEntity>>();
            this.mockTicketRepo = new Mock<IGenericRepository<TicketEntity>>();

            // User repository test settings
            this.userPassword = "somePassword";
            this.jwtTestSecret = "23jrb783v29fwfvfg2874gf286fce8";

            this.options = Options.Create(new AppSettings { JwtSecret = "someSecret" });
        }

    }
}