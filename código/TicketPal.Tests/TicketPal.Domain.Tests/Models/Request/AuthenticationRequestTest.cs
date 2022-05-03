using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TicketPal.Domain.Tests.Models.Request
{
    [TestClass]
    public class AuthenticationRequestTest
    {
        [TestMethod]
        public void EmailNotNull()
        {
            AuthenticationRequest request = new AuthenticationRequest
            {
                Email = "authentication@example.com"
            };
            Assert.IsNotNull(request.Email);
        }

        [TestMethod]
        public void PasswordNotNull()
        {
            AuthenticationRequest request = new AuthenticationRequest
            {
                Password = "myPassword"
            };
            Assert.IsNotNull(request.Password);
        }
    }
}