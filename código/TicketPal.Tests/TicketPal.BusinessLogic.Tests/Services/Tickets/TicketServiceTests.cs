using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketPal.BusinessLogic.Services.Tickets;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Exceptions;
using TicketPal.Domain.Models.Request;
using TicketPal.Domain.Models.Response;

namespace TicketPal.BusinessLogic.Tests.Services.Tickets
{
    [TestClass]
    public class TicketServiceTests : BaseServiceTests
    {
        private TicketEntity ticket;
        private ConcertEntity concert;
        private UserEntity user;
        private AddTicketRequest ticketRequest;
        private TicketService ticketService;


        [TestInitialize]
        public void Initialize()
        {
            ticket = new TicketEntity()
            {
                Id = 1,
                Buyer = user,
                Code = "438tgyhafnisdkgfbo2847gbae9ouf",
                Event = concert,
                PurchaseDate = DateTime.Now,
                Status = TicketStatus.PURCHASED
            };

            ticketRequest = new AddTicketRequest()
            {
                BuyerFirstName = user.Firstname,
                BuyerLastName = user.Lastname,
                BuyerEmail = user.Email
            };

            this.ticketsMock.Setup(m => m.Exists(It.IsAny<int>())).Returns(false);
            this.ticketsMock.Setup(m => m.Add(It.IsAny<TicketEntity>())).Verifiable();

            ticketService = new TicketService(this.ticketsMock.Object, this.testAppSettings, this.mapper);
        }

        [TestMethod]
        public void AddTicketSuccesfullyTest()
        {
            OperationResult result = ticketService.AddTicket(ticketRequest);

            Assert.IsTrue(result.ResultCode == ResultCode.SUCCESS);
        }

        [TestMethod]
        public void AddTicketTwiceFailsTest()
        {
            ticketService.AddTicket(ticketRequest);

            this.ticketsMock.Setup(m => m.Exists(It.IsAny<int>())).Returns(true);
            this.ticketsMock.Setup(m => m.Add(It.IsAny<TicketEntity>())).Throws(new RepositoryException());
            ticketService = new TicketService(this.ticketsMock.Object, this.testAppSettings, this.mapper);

            OperationResult result = ticketService.AddTicket(ticketRequest);

            Assert.IsTrue(result.ResultCode == ResultCode.FAIL);
        }

        [TestMethod]
        public void DeleteTicketSuccesfullyTest()
        {
            var id = 1;
            var dbUser = new TicketEntity
            {
                Id = id,
                Buyer = ticket.Buyer,
                Code = ticket.Code,
                Event = ticket.Event,
                PurchaseDate = ticket.PurchaseDate,
                Status = ticket.Status
            };

            this.ticketsMock.Setup(m => m.Get(It.IsAny<int>())).Returns(dbUser);
            OperationResult result = ticketService.DeleteTicket(id);

            Assert.IsTrue(result.ResultCode == ResultCode.SUCCESS);
        }

        [TestMethod]
        public void DeleteUnexistentTicketFailsTest()
        {
            var id = 1;

            this.ticketsMock.Setup(m => m.Delete(It.IsAny<int>())).Throws(new RepositoryException());
            this.ticketService = new TicketService(this.ticketsMock.Object, this.testAppSettings, this.mapper);
            OperationResult result = ticketService.DeleteTicket(id);

            Assert.IsTrue(result.ResultCode == ResultCode.FAIL);
        }

        [TestMethod]
        public void UpdateTicketSuccesfullyTest()
        {
            var updateRequest = new UpdateTicketRequest
            {
                Code = ticket.Code,
                Status = ticket.Status
            };

            this.ticketsMock.Setup(m => m.Update(It.IsAny<TicketEntity>())).Verifiable();
            OperationResult expected = ticketService.UpdateTicket(updateRequest);

            Assert.IsTrue(expected.ResultCode == ResultCode.SUCCESS);
        }

        [TestMethod]
        public void GetTicketByIdTest()
        {
            int id = 1;
            var dbUser = new TicketEntity
            {
                Id = id,
                Buyer = this.ticket.Buyer,
                Code = this.ticket.Code,
                Event = this.ticket.Event,
                PurchaseDate = this.ticket.PurchaseDate,
                Status = this.ticket.Status
            };

            this.ticketsMock.Setup(r => r.Get(It.IsAny<int>())).Returns(dbUser);
            this.ticketService = new TicketService(this.ticketsMock.Object, this.testAppSettings, this.mapper);
            Ticket ticket = ticketService.GetTicket(id);

            Assert.IsNotNull(ticket);
            Assert.IsTrue(id == ticket.Id);
        }

        [TestMethod]
        public void GetTicketByNullIdTest()
        {
            int id = 1;

            TicketEntity dbUser = null;

            this.ticketsMock.Setup(r => r.Get(It.IsAny<int>())).Returns(dbUser);
            this.ticketService = new TicketService(this.ticketsMock.Object, this.testAppSettings, this.mapper);
            Ticket ticket = ticketService.GetTicket(id);

            Assert.IsNull(ticket);

        }

        [TestMethod]
        public void GetAllTicketsSuccesfullyTest()
        {
            IEnumerable<TicketEntity> dbAccounts = new List<TicketEntity>()
            {
                new TicketEntity
                {
                    Id = 1,
                    Buyer = ticket.Buyer,
                    Code = ticket.Code,
                    Event = ticket.Event,
                    PurchaseDate = ticket.PurchaseDate,
                    Status = ticket.Status
                },
                new TicketEntity
                {
                    Id = 2,
                    Buyer = ticket.Buyer,
                    Code = "nc934hg9q23runvgq0284daf27gd",
                    Event = ticket.Event,
                    PurchaseDate = ticket.PurchaseDate.AddDays(-30),
                    Status = ticket.Status
                },
                new TicketEntity
                {
                    Id = 3,
                    Buyer = ticket.Buyer,
                    Code = "FMN0438abuutfv9q483ghwvnod4536u4w6ht",
                    Event = ticket.Event,
                    PurchaseDate = DateTime.Now.AddDays(-120),
                    Status = TicketStatus.USED
                },
            };

            this.ticketsMock.Setup(r => r.GetAll()).Returns(dbAccounts);
            this.ticketService = new TicketService(this.ticketsMock.Object, this.testAppSettings, this.mapper);
            IEnumerable<Ticket> result = ticketService.GetTickets();

            Assert.IsTrue(result.ToList().Count == 3);
        }
    }
}
