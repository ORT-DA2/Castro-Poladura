using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Models.Request;

namespace TicketPal.Domain.Tests.Models.Request
{
    [TestClass]
    public class SignInRequestTest
    {
        SignUpRequest signInRequest;

        [TestInitialize]
        public void SetUp()
        {
            signInRequest = new SignUpRequest
            {
                Firstname = "John",
                Lastname = "Doe",
                Email = "someone@example.com",
                Password = "myTestEnteredPassword",
                Role = UserRole.SPECTATOR.ToString()
            };
        }

        [TestMethod]
        public void CheckNameEquals()
        {
            Assert.AreEqual("John", signInRequest.Firstname);

        }

        [TestMethod]
        public void CheckAccountEmail_CorrectEmail()
        {
            Assert.AreEqual("someone@example.com", signInRequest.Email);
        }

        [TestMethod]
        public void CheckAccountEmail_IncorrectEmail()
        {
            Assert.AreNotEqual("anaccount@example", signInRequest.Email);
        }

        [TestMethod]
        public void CheckAccountRole_CorrectRole()
        {
            Assert.AreEqual(UserRole.SPECTATOR.ToString(), signInRequest.Role.ToString());
        }

        [TestMethod]
        public void CheckAccountRole_IncorrectRole()
        {
            Assert.AreNotEqual("Role2", signInRequest.Role.ToString());
        }

        [TestMethod]
        public void CheckAccountPassword_CorrectPassword()
        {
            Assert.AreEqual("myTestEnteredPassword", signInRequest.Password);
        }

        [TestMethod]
        public void CheckAccountPassword_IncorrectPassword()
        {
            Assert.AreNotEqual("mypassword", signInRequest.Password);
        }
    }
}