using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Models.Request;
using TicketPal.Domain.Models.Response;
using TicketPal.Interfaces.Services.Tickets;
using TicketPal.Interfaces.Services.Users;
using TicketPal.WebApi.Controllers;

namespace TicketPal.WebApi.Tests.Controllers
{
    [TestClass]
    public class TicketControllerTest
    {
        private Mock<ITicketService> mockTicketService;
        private Mock<IUserService> mockUserService;
        private List<Ticket> tickets;
        private TicketController controller;

        [TestInitialize]
        public void TestSetup()
        {
            mockTicketService = new Mock<ITicketService>(MockBehavior.Default);
            mockUserService = new Mock<IUserService>(MockBehavior.Default);
            controller = new TicketController(mockTicketService.Object);
            this.tickets = SetupTickets();
        }

        [TestMethod]
        public void AddTicketTestOk()
        {
            var request = new AddTicketRequest
            {
                User = new User(),
                Event = 2
            };

            var operationResult = new OperationResult
            {
                ResultCode = ResultCode.SUCCESS,
                Message = "success"
            };

            mockTicketService.Setup(s => s.AddTicket(request)).Returns(operationResult);

            var result = controller.AddTicket(request);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public void AddTicketTestBadRequest()
        {
            var request = new AddTicketRequest
            {
                User = It.IsAny<User>(),
                Event = It.IsAny<int>()
            };

            var operationResult = new OperationResult
            {
                ResultCode = ResultCode.FAIL,
                Message = "error message"
            };

            mockTicketService.Setup(s => s.AddTicket(request)).Returns(operationResult);

            var result = controller.AddTicket(request);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(400, statusCode);
        }

        [TestMethod]
        public void UpdateTicketOkTest()
        {
            var request = new UpdateTicketRequest
            {
                Id = 2,
                Code = It.IsAny<string>(),
                Status = TicketStatus.PURCHASED
            };

            var operationResult = new OperationResult
            {
                ResultCode = ResultCode.SUCCESS,
                Message = "success"
            };

            mockTicketService.Setup(s => s.UpdateTicket(request)).Returns(operationResult);

            var result = controller.UpdateTicket(It.IsAny<int>(),request);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public void UpdateTicketBadRequestTest()
        {
            var request = new UpdateTicketRequest
            {
                Id = 2,
                Code = It.IsAny<string>(),
                Status = TicketStatus.PURCHASED
            };

            var operationResult = new OperationResult
            {
                ResultCode = ResultCode.FAIL,
                Message = "error message"
            };

            mockTicketService.Setup(s => s.UpdateTicket(request)).Returns(operationResult);

            var result = controller.UpdateTicket(It.IsAny<int>(),request);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(400, statusCode);
        }

        [TestMethod]
        public void DeleteTicketTest()
        {
            var operationResult = new OperationResult
            {
                ResultCode = ResultCode.SUCCESS,
                Message = "success message"
            };

            mockTicketService.Setup(s => s.DeleteTicket(It.IsAny<int>())).Returns(operationResult);

            var account = controller.DeleteTicket(It.IsAny<int>());

            var objectResult = account as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        public void DeleteTicketBadRequestTest()
        {
            var operationResult = new OperationResult
            {
                ResultCode = ResultCode.FAIL,
                Message = "some error"
            };

            mockTicketService.Setup(s => s.DeleteTicket(It.IsAny<int>())).Returns(operationResult);

            var account = controller.DeleteTicket(It.IsAny<int>());

            var objectResult = account as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        public void GetTicketsIfAdminOkTest()
        {
            var mockHttpContext = new Mock<HttpContext>();
            var mockHeaderHttp = new Mock<IHeaderDictionary>();
            mockHeaderHttp.Setup(x => x[It.IsAny<string>()]).Returns("someHeader");
            var mockHttpRequest = new Mock<HttpRequest>();
            mockHttpRequest.Setup(s => s.Headers).Returns(mockHeaderHttp.Object);
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockHttpContext.Setup(s => s.Request).Returns(mockHttpRequest.Object);
            mockUserService.Setup(s => s.RetrieveUserFromToken(It.IsAny<string>())).Returns(
                new User {
                    Role = UserRole.ADMIN.ToString(),
                    Id = 1,
                });
            mockHttpContext.Setup(x => x.RequestServices.GetService(typeof(IUserService)))
                .Returns(mockUserService.Object);

            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            mockTicketService.Setup(s => s.GetTickets()).Returns(tickets);

            var account = controller.GetTickets();

            var objectResult = account as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        public void GetTicketsIfSameUserOkTest()
        {
            var mockHttpContext = new Mock<HttpContext>();
            var mockHeaderHttp = new Mock<IHeaderDictionary>();
            mockHeaderHttp.Setup(x => x[It.IsAny<string>()]).Returns("someHeader");
            var mockHttpRequest = new Mock<HttpRequest>();
            mockHttpRequest.Setup(s => s.Headers).Returns(mockHeaderHttp.Object);
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockHttpContext.Setup(s => s.Request).Returns(mockHttpRequest.Object);
            mockUserService.Setup(s => s.RetrieveUserFromToken(It.IsAny<string>())).Returns(tickets[0].Buyer);
            mockHttpContext.Setup(x => x.RequestServices.GetService(typeof(IUserService)))
                .Returns(mockUserService.Object);

            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            mockTicketService.Setup(s => s.GetUserTickets(1)).Returns(tickets);

            var account = controller.GetTickets();

            var objectResult = account as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        public void GetTicketOkTest()
        {
            mockTicketService.Setup(s => s.GetTicket(It.IsAny<int>())).Returns(tickets[0]);

            var account = controller.GetTicket(It.IsAny<int>());

            var objectResult = account as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        private List<Ticket> SetupTickets()
        {
            return new List<Ticket>
            {
                new Ticket {
                    Buyer = new User { Role = UserRole.SPECTATOR.ToString(), Id = 1},
                    Event = new Concert{
                        Id = 1,
                        Date = DateTime.Now,
                        AvailableTickets = 2,
                        TicketPrice = 200M,
                        CurrencyType = CurrencyType.UYU,
                        EventType = EventType.CONCERT,
                        TourName = "SomeName",
                        Artist = new Performer {
                            Id = 4,
                            Name = "SomeName",
                            PerformerType = PerformerType.SOLO_ARTIST,
                            StartYear = "1987",
                            Genre = new Genre {GenreName = "Pop"},
                            Artists = "someName|anotherName|other"
                        }
                    }
                },
                new Ticket {
                    Buyer = new User { Role = UserRole.ADMIN.ToString(), Id = 2},
                    Event = new Concert{
                        Id = 2,
                        Date = DateTime.Now,
                        AvailableTickets = 6,
                        TicketPrice = 188M,
                        CurrencyType = CurrencyType.USD,
                        EventType = EventType.CONCERT,
                        TourName = "A tour name",
                        Artist = new Performer {
                            Id = 3,
                            Name = "SomeName",
                            PerformerType = PerformerType.SOLO_ARTIST,
                            StartYear = "1987",
                            Genre = new Genre {GenreName = "Rock"},
                            Artists = "someName|anotherName|other"
                        }
                    }
                }
            };
        }
    }
}