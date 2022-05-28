using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicketPal.Domain.Models.Response;

namespace TicketPal.Domain.Tests.Models.Response
{
    [TestClass]
    public class UserTest
    {
        User accountToBeTested;

        [TestInitialize]
        public void SetUp()
        {
            accountToBeTested = new User
            {
                Id = 1,
                Firstname = "someName",
                Lastname = "someLastname",
                Email = "myaccount@example.com",
                Password = "myPassword",
                Token = "UcuJ7gBX87",
                Role = "Role1"
            };
        }

        [TestMethod]
        public void CheckAccountId_CorrectId()
        {
            Assert.AreEqual(1, accountToBeTested.Id);
        }

        [TestMethod]
        public void CheckFirstnameEquals()
        {
            Assert.AreEqual("someName", accountToBeTested.Firstname);

        }

        [TestMethod]
        public void CheckLastnameEquals()
        {
            Assert.AreEqual("someLastname", accountToBeTested.Lastname);

        }

        [TestMethod]
        public void CheckAccountId_IncorrectId()
        {
            Assert.AreNotEqual(2, accountToBeTested.Id);
        }

        [TestMethod]
        public void CheckAccountEmail_CorrectEmail()
        {
            Assert.AreEqual("myaccount@example.com", accountToBeTested.Email);
        }

        [TestMethod]
        public void CheckAccountEmail_IncorrectEmail()
        {
            Assert.AreNotEqual("anaccount@example", accountToBeTested.Email);
        }

        [TestMethod]
        public void CheckAccountToken_CorrectToken()
        {
            Assert.AreEqual("UcuJ7gBX87", accountToBeTested.Token);
        }

        [TestMethod]
        public void CheckAccountToken_IncorrectToken()
        {
            Assert.AreNotEqual("LcuJ8gBX87", accountToBeTested.Token);
        }

        [TestMethod]
        public void CheckAccountRole_CorrectRole()
        {
            Assert.AreEqual("Role1", accountToBeTested.Role);
        }

        [TestMethod]
        public void CheckAccountRole_IncorrectRole()
        {
            Assert.AreNotEqual("Role2", accountToBeTested.Role);
        }

        [TestMethod]
        public void CheckAccountPassword_CorrectPassword()
        {
            Assert.AreEqual("myPassword", accountToBeTested.Password);
        }

        [TestMethod]
        public void CheckAccountPassword_IncorrectPassword()
        {
            Assert.AreNotEqual("mypassword", accountToBeTested.Password);
        }
    }
}