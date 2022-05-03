using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Models.Response;

namespace TicketPal.Domain.Tests.Models.Response
{
    [TestClass]
    public class OperationResultTest
    {

        [TestMethod]
        public void OperationResultNotNull()
        {
            OperationResult result = new OperationResult();

            Assert.IsNotNull(result);
        }


        [TestMethod]
        public void SetMessageNotNull()
        {
            OperationResult result = new OperationResult
            {
                Message = "sample message"
            };

            Assert.IsNotNull(result.Message);
        }

        [TestMethod]
        public void SetMessageEquals()
        {
            string message = "sample message";

            OperationResult result = new OperationResult
            {
                Message = message
            };

            Assert.IsTrue(result.Message.Equals(message));
        }

        [TestMethod]
        public void SetResultCodeSuccess()
        {

            OperationResult result = new OperationResult
            {
                ResultCode = ResultCode.SUCCESS
            };

            Assert.IsTrue(result.ResultCode == ResultCode.SUCCESS);
        }

        [TestMethod]
        public void SetResultCodeFail()
        {

            OperationResult result = new OperationResult
            {
                ResultCode = ResultCode.FAIL
            };

            Assert.IsTrue(result.ResultCode == ResultCode.FAIL);
        }
    }
}
