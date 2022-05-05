using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Models.Request;

namespace TicketPal.Domain.Tests.Models.Request
{
    [TestClass]
    public class UpdateUserRequestTest
    {
        [TestMethod]
        public void EmailNotNull()
        {
            UpdateUserRequest request = new UpdateUserRequest
            {
                Email = "request@example.com"
            };

            Assert.IsNotNull(request.Email);
        }

        [TestMethod]
        public void PasswordNotNull()
        {
            UpdateUserRequest request = new UpdateUserRequest
            {
                Password = "myPassword"
            };

            Assert.IsNotNull(request.Password);
        }

        [TestMethod]
        public void NameNotNull()
        {
            UpdateUserRequest request = new UpdateUserRequest
            {
                Firstname = "someName"
            };

            Assert.IsNotNull(request.Firstname);
        }

        [TestMethod]
        public void LastnameNotNull()
        {
            UpdateUserRequest request = new UpdateUserRequest
            {
                Lastname = "someName"
            };

            Assert.IsNotNull(request.Lastname);
        }

        [TestMethod]
        public void RoleNotNull()
        {
            UpdateUserRequest request = new UpdateUserRequest
            {
                Role = UserRole.ADMIN.ToString()
            };

            Assert.IsNotNull(request.Role);
        }

    }
}