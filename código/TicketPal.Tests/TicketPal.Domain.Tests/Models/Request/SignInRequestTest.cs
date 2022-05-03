using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TicketPal.Domain.Tests.Models.Request
{
    [TestClass]
    public class SignInRequestTest
    {
        SignInRequest signInRequest;

        [TestInitialize]
        public void SetUp()
        {
            signInRequest = new SignInRequest
            {
                Firstname = "John",
                Lastname = "Doe",
                Email = "someone@example.com",
                Password = "myTestEnteredPassword",
                Role = UserRoles.SPECTATOR
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
            Assert.AreEqual(UserRoles.SPECTATOR, signInRequest.Role);
        }

        [TestMethod]
        public void CheckAccountRole_IncorrectRole()
        {
            Assert.AreNotEqual("Role2", signInRequest.Role);
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