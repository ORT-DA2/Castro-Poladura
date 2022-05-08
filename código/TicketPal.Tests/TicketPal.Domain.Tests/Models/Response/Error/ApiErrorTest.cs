using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TicketPal.Domain.Tests.Models.Response.Error
{
    [TestClass]
    public class ApiErrorTest
    {
        [TestMethod]
        public void StatusCodeCheckEquals()
        {
            var error = new ApiError(200, "OK");
            Assert.AreEqual(200, error.StatusCode);
        }

        [TestMethod]
        public void StatusCodeDescriptionCheckEquals()
        {
            var desc = "OK";
            var error = new ApiError(200, desc);
            Assert.AreEqual(desc, error.StatusDescription);
        }

        [TestMethod]
        public void StatusMessageCheckEquals()
        {
            var desc = "OK";
            var message = "some error message";
            var error = new ApiError(200, desc, message);
            Assert.AreEqual(message, error.Message);
        }
    }
}