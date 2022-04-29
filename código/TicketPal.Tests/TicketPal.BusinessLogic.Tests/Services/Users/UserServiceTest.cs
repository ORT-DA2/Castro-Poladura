
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TicketPal.BusinessLogic.Tests.Services.Users 
{
    [TestClass]
    public class UserServiceTest
    {
        private string jwtTestSecret;
        private string userPassword;
        private IUserService userService;
        private IUnitOfWork uow;
        private Mock<IRepositoryWrapper> mockWrapper;
        private IMapper mapper;
    }
}