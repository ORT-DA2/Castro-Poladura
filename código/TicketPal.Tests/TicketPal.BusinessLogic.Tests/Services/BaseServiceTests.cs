using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TicketPal.BusinessLogic.Mapper;
using TicketPal.BusinessLogic.Settings.Api;
using TicketPal.Interfaces.Repository;
using TicketPal.Interfaces.Services.Users;

namespace TicketPal.BusinessLogic.Tests.Services
{
    [TestClass]
    public abstract class BaseServiceTests
    {
        protected Mock<DbContext> mockDbContext;
        protected IMapper mapper;
        // Repository mocks
        protected Mock<IUserRepository> usersMock;
        protected Mock<IConcertRepository> concertsMock;
        protected Mock<IGenreRepository> genresMock;
        protected Mock<IPerformerRepository> performersMock;
        protected Mock<ITicketRepository> ticketsMock;
        // Services
        protected IUserService userService;
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

            // Repositories mock
            this.usersMock = new Mock<IUserRepository>();
            this.concertsMock = new Mock<IConcertRepository>();
            this.genresMock = new Mock<IGenreRepository>();
            this.performersMock = new Mock<IPerformerRepository>();
            this.ticketsMock = new Mock<ITicketRepository>();
            // User repository test settings
            this.userPassword = "somePassword";
            this.jwtTestSecret = "23jrb783v29fwfvfg2874gf286fce8";
            this.testAppSettings = Options.Create(new AppSettings { JwtSecret = jwtTestSecret });
        }

    }
}