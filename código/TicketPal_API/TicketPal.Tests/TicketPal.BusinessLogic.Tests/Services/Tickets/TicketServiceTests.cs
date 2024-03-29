﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TicketPal.BusinessLogic.Services.Tickets;
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
                Name = "Rock"
            };

            artist = new PerformerEntity()
            {
                Id = 214,
                UserInfo = new UserEntity { Firstname = "Bryan Adams" },
                Genre = genre,
                PerformerType = Constants.PERFORMER_TYPE_SOLO_ARTIST,
                StartYear = "1978"
            };

            concert = new ConcertEntity()
            {
                Id = 1,
                Artists = new List<PerformerEntity>(),
                AvailableTickets = 2416,
                CurrencyType = Constants.CURRENCY_US_DOLLARS,
                Date = DateTime.Now.AddDays(120),
                EventType = Constants.EVENT_CONCERT_TYPE,
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
                Status = Constants.TICKET_PURCHASED_STATUS
            };

            ticketRequest = new AddTicketRequest()
            {
                EventId = concert.Id,
                LoggedUserId = 1
            };

            this.mockTicketRepo.Setup(m => m.Exists(It.IsAny<int>())).Returns(false);
            this.mockTicketRepo.Setup(m => m.Add(It.IsAny<TicketEntity>())).Verifiable();

            this.mockConcertRepo.Setup(m => m.Get(It.IsAny<int>())).Returns(Task.FromResult(concert));

            this.factoryMock.Setup(m => m.GetRepository(typeof(TicketEntity))).Returns(this.mockTicketRepo.Object);
            this.factoryMock.Setup(m => m.GetRepository(typeof(ConcertEntity))).Returns(this.mockConcertRepo.Object);
            var mockTicketCode = new Mock<ITicketCode>();
            mockTicketCode.Setup(m => m.GenerateTicketCode()).Returns("gq3597gh3948tfhn93QFNeudi");
            this.factoryMock.Setup(m => m.GetService(typeof(ITicketCode))).Returns(mockTicketCode.Object);

            this.ticketService = new TicketService(this.factoryMock.Object, this.mapper);
        }

        [TestMethod]
        public async Task AddTicketSuccesfullyTest()
        {
            ticketRequest.FirstName = "SomeBuyerName";
            ticketRequest.LastName = "SomeBuyerLastname";
            ticketRequest.Email = "buyer@email.com";


            OperationResult result = await ticketService.AddTicket(ticketRequest);

            Assert.IsTrue(result.ResultCode == Constants.CODE_SUCCESS);
        }

        [TestMethod]
        public async Task AddTicketTwiceFailsTest()
        {
            ticketRequest.FirstName = "SomeBuyerName";
            ticketRequest.LastName = "SomeBuyerLastname";
            ticketRequest.Email = "buyer@email.com";

            await ticketService.AddTicket(ticketRequest);

            this.mockTicketRepo.Setup(m => m.Exists(It.IsAny<int>())).Returns(true);
            this.mockTicketRepo.Setup(m => m.Add(It.IsAny<TicketEntity>())).Throws(new RepositoryException());
            this.factoryMock.Setup(m => m.GetRepository(typeof(TicketEntity))).Returns(this.mockTicketRepo.Object);

            this.ticketService = new TicketService(this.factoryMock.Object, this.mapper);

            OperationResult result = await ticketService.AddTicket(ticketRequest);

            Assert.IsTrue(result.ResultCode == Constants.CODE_FAIL);
        }

        [TestMethod]
        public async Task AddTicketWithNoExistentEventTest()
        {
            this.mockConcertRepo.Setup(m => m.Get(It.IsAny<int>()));

            this.factoryMock.Setup(m => m.GetRepository(typeof(TicketEntity))).Returns(this.mockTicketRepo.Object);
            this.factoryMock.Setup(m => m.GetRepository(typeof(ConcertEntity))).Returns(this.mockConcertRepo.Object);
            var mockTicketCode = new Mock<ITicketCode>();
            mockTicketCode.Setup(m => m.GenerateTicketCode()).Returns("gq3597gh3948tfhn93QFNeudi");
            this.factoryMock.Setup(m => m.GetService(typeof(ITicketCode))).Returns(mockTicketCode.Object);

            this.ticketService = new TicketService(this.factoryMock.Object, this.mapper);

            OperationResult result = await ticketService.AddTicket(ticketRequest);

            Assert.IsTrue(result.ResultCode == Constants.CODE_FAIL);
        }

        [TestMethod]
        public async Task DeleteTicketSuccesfullyTest()
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

            this.mockTicketRepo.Setup(m => m.Get(It.IsAny<int>())).Returns(Task.FromResult(dbUser));
            this.factoryMock.Setup(m => m.GetRepository(typeof(TicketEntity))).Returns(this.mockTicketRepo.Object);

            this.ticketService = new TicketService(this.factoryMock.Object, this.mapper);
            OperationResult result = await ticketService.DeleteTicket(id);

            Assert.IsTrue(result.ResultCode == Constants.CODE_SUCCESS);
        }

        [TestMethod]
        public async Task DeleteUnexistentTicketFailsTest()
        {
            var id = 1;

            this.mockTicketRepo.Setup(m => m.Delete(It.IsAny<int>())).Throws(new RepositoryException());
            this.factoryMock.Setup(m => m.GetRepository(typeof(TicketEntity))).Returns(this.mockTicketRepo.Object);

            this.ticketService = new TicketService(this.factoryMock.Object, this.mapper);
            OperationResult result = await ticketService.DeleteTicket(id);

            Assert.IsTrue(result.ResultCode == Constants.CODE_FAIL);
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

            Assert.IsTrue(expected.ResultCode == Constants.CODE_SUCCESS);
        }

        [TestMethod]
        public async Task GetTicketByIdTest()
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

            this.mockTicketRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(Task.FromResult(dbUser));
            this.factoryMock.Setup(m => m.GetRepository(typeof(TicketEntity))).Returns(this.mockTicketRepo.Object);

            this.ticketService = new TicketService(this.factoryMock.Object, this.mapper);
            Ticket ticket = await ticketService.GetTicket(id);

            Assert.IsNotNull(ticket);
            Assert.IsTrue(id == ticket.Id);
        }

        [TestMethod]
        public async Task GetTicketByCodeTest()
        {
            int id = 1;
            var dbTicket = new TicketEntity
            {
                Id = id,
                Buyer = this.ticket.Buyer,
                Code = this.ticket.Code,
                Event = this.ticket.Event,
                PurchaseDate = this.ticket.PurchaseDate,
                Status = this.ticket.Status
            };

            this.mockTicketRepo.Setup(r => r.Get(It.IsAny<Expression<Func<TicketEntity, bool>>>())).Returns(Task.FromResult(dbTicket));
            this.factoryMock.Setup(m => m.GetRepository(typeof(TicketEntity))).Returns(this.mockTicketRepo.Object);

            this.ticketService = new TicketService(this.factoryMock.Object, this.mapper);
            Ticket ticket = await ticketService.GetTicketByCode("someCode");

            Assert.IsNotNull(ticket);
            Assert.IsTrue(id == ticket.Id);
        }

        [TestMethod]
        public async Task GetTicketByNullIdTest()
        {
            int id = 1;

            TicketEntity dbUser = null;

            this.mockTicketRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(Task.FromResult(dbUser));
            this.factoryMock.Setup(m => m.GetRepository(typeof(TicketEntity))).Returns(this.mockTicketRepo.Object);

            this.ticketService = new TicketService(this.factoryMock.Object, this.mapper);
            Ticket ticket = await ticketService.GetTicket(id);

            Assert.IsNull(ticket);

        }

        [TestMethod]
        public async Task GetAllTicketsSuccesfullyTest()
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
                    Status = Constants.TICKET_USED_STATUS
                },
            };

            this.mockTicketRepo.Setup(r => r.GetAll()).Returns(Task.FromResult(dbAccounts.ToList()));
            this.factoryMock.Setup(m => m.GetRepository(typeof(TicketEntity))).Returns(this.mockTicketRepo.Object);

            this.ticketService = new TicketService(this.factoryMock.Object, this.mapper);
            IEnumerable<Ticket> result = await ticketService.GetTickets();

            Assert.IsTrue(result.ToList().Count == 3);
        }

        [TestMethod]
        public async Task GetAllUserTicketsSuccesfullyTest()
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
                    Status = Constants.TICKET_USED_STATUS
                },
            };

            this.mockTicketRepo.Setup(r => r.GetAll(It.IsAny<Expression<Func<TicketEntity, bool>>>())).Returns(Task.FromResult(dbAccounts.ToList()));
            this.factoryMock.Setup(m => m.GetRepository(typeof(TicketEntity))).Returns(this.mockTicketRepo.Object);

            this.ticketService = new TicketService(this.factoryMock.Object, this.mapper);
            IEnumerable<Ticket> result = await ticketService.GetUserTickets(It.IsAny<int>());

            Assert.IsTrue(result.ToList().Count == 3);
        }
    }
}
