using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TicketPal.BusinessLogic.Services.Tickets;
using TicketPal.BusinessLogic.Utils.TicketCodes;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Exceptions;
using TicketPal.Domain.Models.Request;
using TicketPal.Domain.Models.Response;
using TicketPal.Interfaces.Utils.TicketCodes;

namespace TicketPal.BusinessLogic.Tests.Services.Tickets
{
    [TestClass]
    public class TicketServiceTests : BaseServiceTests
    {
        private TicketEntity ticket;
        private GenreEntity genre;
        private PerformerEntity artist;
        private ConcertEntity concert;
        private UserEntity user;
        private User userResponse;
        private AddTicketRequest ticketRequest;
        private TicketService ticketService;


        [TestInitialize]
        public void Initialize()
        {
            user = new UserEntity()
            {
                Id = 81,
                Email = "holamundo@hotmail.com",
                Firstname = "Jack",
                Lastname = "Palance",
                Password = "9284mfqp34r9wagi",
                Role = "SPECTATOR"
            };

            userResponse = new User()
            {
                Id = user.Id,
                Email = user.Email,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Password = user.Password,
                Role = user.Role,
                Token = "247yftgh4wyr8sfdubifkgqf3r97gh3q7gbiruy954",
                ActiveAccount = true
            };

            genre = new GenreEntity()
            {
                Id = 36,
                GenreName = "Rock"
            };

            artist = new PerformerEntity()
            {
                Id = 214,
                Name = "Bryan Adams",
                Genre = genre,
                PerformerType = PerformerType.SOLO_ARTIST,
                StartYear = "1978"
            };

            concert = new ConcertEntity()
            {
                Id = 1,
                Artist = artist,
                AvailableTickets = 2416,
                CurrencyType = CurrencyType.USD,
                Date = DateTime.Now.AddDays(120),
                EventType = EventType.CONCERT,
                TicketPrice = 258,
                TourName = "The World"
            };

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
                Event = concert.Id,
                User = userResponse
            };

            this.mockTicketRepo.Setup(m => m.Exists(It.IsAny<int>())).Returns(false);
            this.mockTicketRepo.Setup(m => m.Add(It.IsAny<TicketEntity>())).Verifiable();

            this.mockConcertRepo.Setup(m => m.Get(It.IsAny<int>())).Returns(concert);

            this.factoryMock.Setup(m => m.GetRepository(typeof(TicketEntity))).Returns(this.mockTicketRepo.Object);
            this.factoryMock.Setup(m => m.GetRepository(typeof(ConcertEntity))).Returns(this.mockConcertRepo.Object);
            var mockTicketCode = new Mock<ITicketCode>();
            mockTicketCode.Setup(m => m.GenerateTicketCode()).Returns("gq3597gh3948tfhn93QFNeudi");
            this.factoryMock.Setup(m => m.GetService(typeof(ITicketCode))).Returns(mockTicketCode.Object);

            this.ticketService = new TicketService(this.factoryMock.Object, this.mapper);
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

            this.mockTicketRepo.Setup(m => m.Exists(It.IsAny<int>())).Returns(true);
            this.mockTicketRepo.Setup(m => m.Add(It.IsAny<TicketEntity>())).Throws(new RepositoryException());
            this.factoryMock.Setup(m => m.GetRepository(typeof(TicketEntity))).Returns(this.mockTicketRepo.Object);

            this.ticketService = new TicketService(this.factoryMock.Object, this.mapper);

            OperationResult result = ticketService.AddTicket(ticketRequest);

            Assert.IsTrue(result.ResultCode == ResultCode.FAIL);
        }

        [TestMethod]
        public void AddTicketWithNoExistentEventTest()
        {
            this.mockConcertRepo.Setup(m => m.Get(It.IsAny<int>()));

            this.factoryMock.Setup(m => m.GetRepository(typeof(TicketEntity))).Returns(this.mockTicketRepo.Object);
            this.factoryMock.Setup(m => m.GetRepository(typeof(ConcertEntity))).Returns(this.mockConcertRepo.Object);
            var mockTicketCode = new Mock<ITicketCode>();
            mockTicketCode.Setup(m => m.GenerateTicketCode()).Returns("gq3597gh3948tfhn93QFNeudi");
            this.factoryMock.Setup(m => m.GetService(typeof(ITicketCode))).Returns(mockTicketCode.Object);

            this.ticketService = new TicketService(this.factoryMock.Object, this.mapper);

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

            this.mockTicketRepo.Setup(m => m.Get(It.IsAny<int>())).Returns(dbUser);
            this.factoryMock.Setup(m => m.GetRepository(typeof(TicketEntity))).Returns(this.mockTicketRepo.Object);

            this.ticketService = new TicketService(this.factoryMock.Object, this.mapper);
            OperationResult result = ticketService.DeleteTicket(id);

            Assert.IsTrue(result.ResultCode == ResultCode.SUCCESS);
        }

        [TestMethod]
        public void DeleteUnexistentTicketFailsTest()
        {
            var id = 1;

            this.mockTicketRepo.Setup(m => m.Delete(It.IsAny<int>())).Throws(new RepositoryException());
            this.factoryMock.Setup(m => m.GetRepository(typeof(TicketEntity))).Returns(this.mockTicketRepo.Object);

            this.ticketService = new TicketService(this.factoryMock.Object, this.mapper);
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

            this.mockTicketRepo.Setup(m => m.Update(It.IsAny<TicketEntity>())).Verifiable();
            this.factoryMock.Setup(m => m.GetRepository(typeof(TicketEntity))).Returns(this.mockTicketRepo.Object);

            this.ticketService = new TicketService(this.factoryMock.Object, this.mapper);
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

            this.mockTicketRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(dbUser);
            this.factoryMock.Setup(m => m.GetRepository(typeof(TicketEntity))).Returns(this.mockTicketRepo.Object);

            this.ticketService = new TicketService(this.factoryMock.Object, this.mapper);
            Ticket ticket = ticketService.GetTicket(id);

            Assert.IsNotNull(ticket);
            Assert.IsTrue(id == ticket.Id);
        }

        [TestMethod]
        public void GetTicketByNullIdTest()
        {
            int id = 1;

            TicketEntity dbUser = null;

            this.mockTicketRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(dbUser);
            this.factoryMock.Setup(m => m.GetRepository(typeof(TicketEntity))).Returns(this.mockTicketRepo.Object);

            this.ticketService = new TicketService(this.factoryMock.Object, this.mapper);
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

            this.mockTicketRepo.Setup(r => r.GetAll()).Returns(dbAccounts);
            this.factoryMock.Setup(m => m.GetRepository(typeof(TicketEntity))).Returns(this.mockTicketRepo.Object);

            this.ticketService = new TicketService(this.factoryMock.Object, this.mapper);
            IEnumerable<Ticket> result = ticketService.GetTickets();

            Assert.IsTrue(result.ToList().Count == 3);
        }

        [TestMethod]
        public void GetAllUserTicketsSuccesfullyTest()
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

            this.mockTicketRepo.Setup(r => r.GetAll(It.IsAny<Expression<Func<TicketEntity, bool>>>())).Returns(dbAccounts);
            this.factoryMock.Setup(m => m.GetRepository(typeof(TicketEntity))).Returns(this.mockTicketRepo.Object);

            this.ticketService = new TicketService(this.factoryMock.Object, this.mapper);
            IEnumerable<Ticket> result = ticketService.GetUserTickets(It.IsAny<int>());

            Assert.IsTrue(result.ToList().Count == 3);
        }
    }
}
