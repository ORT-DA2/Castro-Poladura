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
        private Mock<HttpContext> mockHttpContext;
        private List<Ticket> tickets;
        private TicketsController controller;

        [TestInitialize]
        public void TestSetup()
        {
            mockUserService = new Mock<IUserService>(MockBehavior.Default);
            mockTicketService = new Mock<ITicketService>(MockBehavior.Default);
            mockHttpContext = new Mock<HttpContext>(MockBehavior.Default);

            var mockHeaderHttp = new Mock<IHeaderDictionary>();
            mockHeaderHttp.Setup(x => x[It.IsAny<string>()]).Returns("someHeader");
            var mockHttpRequest = new Mock<HttpRequest>();
            mockHttpRequest.Setup(s => s.Headers).Returns(mockHeaderHttp.Object);
            mockHttpContext.Setup(s => s.Request).Returns(mockHttpRequest.Object);


            controller = new TicketsController(mockTicketService.Object);

            this.tickets = SetupTickets();
        }

        [TestMethod]
        public void AddTicketTestOk()
        {
            var request = new AddTicketRequest
            {
                LoggedUserId = 1,
                EventId = 2
            };

            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_SUCCESS,
                Message = "success"
            };

            mockUserService.Setup(s => s.RetrieveUserFromToken(It.IsAny<string>())).Returns(tickets[0].Buyer);
            mockHttpContext.Setup(x => x.RequestServices.GetService(typeof(IUserService)))
                .Returns(mockUserService.Object);
            mockTicketService.Setup(s => s.AddTicket(request)).Returns(operationResult);
            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            var result = controller.AddTicket(It.IsAny<int>(), request);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public void AddTicketTestBadRequest()
        {
            var request = new AddTicketRequest
            {
                LoggedUserId = 1,
                EventId = It.IsAny<int>()
            };

            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_FAIL,
                Message = "error message"
            };

            mockUserService.Setup(s => s.RetrieveUserFromToken(It.IsAny<string>())).Returns(tickets[0].Buyer);
            mockHttpContext.Setup(x => x.RequestServices.GetService(typeof(IUserService)))
                .Returns(mockUserService.Object);
            mockTicketService.Setup(s => s.AddTicket(request)).Returns(operationResult);
            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            var result = controller.AddTicket(It.IsAny<int>(), request);
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
                Status = Constants.TICKET_PURCHASED_STATUS
            };

            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_SUCCESS,
                Message = "success"
            };

            mockTicketService.Setup(s => s.UpdateTicket(request)).Returns(operationResult);

            var result = controller.UpdateTicket(It.IsAny<int>(), request);
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
                Status = Constants.TICKET_PURCHASED_STATUS
            };

            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_FAIL,
                Message = "error message"
            };

            mockTicketService.Setup(s => s.UpdateTicket(request)).Returns(operationResult);

            var result = controller.UpdateTicket(It.IsAny<int>(), request);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(400, statusCode);
        }

        [TestMethod]
        public void DeleteTicketTest()
        {
            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_SUCCESS,
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
                ResultCode = Constants.CODE_FAIL,
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
                new User
                {
                    Role = Constants.ROLE_ADMIN,
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
                    Buyer = new User { Role = Constants.ROLE_SPECTATOR, Id = 1},
                    Event = new Concert{
                        Id = 1,
                        Date = DateTime.Now,
                        AvailableTickets = 2,
                        TicketPrice = 200M,
                        CurrencyType = Constants.CURRENCY_URUGUAYAN_PESO,
                        EventType = Constants.EVENT_CONCERT_TYPE,
                        TourName = "SomeName"
                    }
                },
                new Ticket {
                    Buyer = new User { Role = Constants.ROLE_ADMIN, Id = 2},
                    Event = new Concert{
                        Id = 2,
                        Date = DateTime.Now,
                        AvailableTickets = 6,
                        TicketPrice = 188M,
                        CurrencyType = Constants.CURRENCY_US_DOLLARS,
                        EventType = Constants.EVENT_CONCERT_TYPE,
                        TourName = "A tour name"
                    }
                }
            };
        }
    }
}