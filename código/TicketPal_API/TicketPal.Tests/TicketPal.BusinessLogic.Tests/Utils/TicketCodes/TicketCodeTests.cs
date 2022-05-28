using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketPal.BusinessLogic.Utils.TicketCodes;
using TicketPal.Interfaces.Utils.TicketCodes;

namespace TicketPal.BusinessLogic.Tests.Utils.TicketCodes
{
    [TestClass]
    public class TicketCodeTests
    {
        private ITicketCode ticketCode;

        [TestInitialize]
        public void Initialize()
        {
            ticketCode = new TicketCode();
        }

        [TestMethod]
        public void GenerateTicketCodeSuccesfully()
        {
            string code = ticketCode.GenerateTicketCode();

            Assert.AreNotEqual(code, null);
        }
    }
}
