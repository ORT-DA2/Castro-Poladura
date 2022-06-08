using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
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
        public async Task AddTicketTestOk()
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

            var sessionMock = new Mock<ISession>();
            var userJson = JsonConvert.SerializeObject(tickets[0].Buyer);
            var value = Encoding.UTF8.GetBytes(userJson);

            sessionMock.Setup(_ => _.Set(It.IsAny<string>(), It.IsAny<byte[]>()))
                .Callback<string, byte[]>((k, v) => value = v);
            sessionMock.Setup(_ => _.TryGetValue(It.IsAny<string>(), out value))
                .Returns(true);
            mockHttpContext.Setup(s => s.Session).Returns(sessionMock.Object);
            mockTicketService.Setup(s => s.AddTicket(request)).Returns(Task.FromResult(operationResult));

            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            var result = await controller.AddTicket(It.IsAny<int>(), request);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public async Task AddTicketTestBadRequest()
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

            var sessionMock = new Mock<ISession>();
            var userJson = JsonConvert.SerializeObject(tickets[0].Buyer);
            var value = Encoding.UTF8.GetBytes(userJson);

            sessionMock.Setup(_ => _.Set(It.IsAny<string>(), It.IsAny<byte[]>()))
                .Callback<string, byte[]>((k, v) => value = v);
            sessionMock.Setup(_ => _.TryGetValue(It.IsAny<string>(), out value))
                .Returns(true);
            mockHttpContext.Setup(s => s.Session).Returns(sessionMock.Object);

            mockTicketService.Setup(s => s.AddTicket(request)).Returns(Task.FromResult(operationResult));
            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            var result = await controller.AddTicket(It.IsAny<int>(), request);
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
        public async Task DeleteTicketTest()
        {
            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_SUCCESS,
                Message = "success message"
            };

            mockTicketService.Setup(s => s.DeleteTicket(It.IsAny<int>())).Returns(Task.FromResult(operationResult));

            var account = await controller.DeleteTicket(It.IsAny<int>());

            var objectResult = account as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        public async Task DeleteTicketBadRequestTest()
        {
            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_FAIL,
                Message = "some error"
            };

            mockTicketService.Setup(s => s.DeleteTicket(It.IsAny<int>())).Returns(Task.FromResult(operationResult));

            var account = await controller.DeleteTicket(It.IsAny<int>());

            var objectResult = account as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        public async Task GetTicketsIfAdminOkTest()
        {
            var mockHttpContext = new Mock<HttpContext>();
            var mockHeaderHttp = new Mock<IHeaderDictionary>();
            mockHeaderHttp.Setup(x => x[It.IsAny<string>()]).Returns("someHeader");
            var mockHttpRequest = new Mock<HttpRequest>();
            mockHttpRequest.Setup(s => s.Headers).Returns(mockHeaderHttp.Object);
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockHttpContext.Setup(s => s.Request).Returns(mockHttpRequest.Object);
            mockUserService.Setup(s => s.RetrieveUserFromToken(It.IsAny<string>())).Returns(
                Task.FromResult(
                    new User
                    {
                        Role = Constants.ROLE_ADMIN,
                        Id = 1,
                    }
                ));
            mockHttpContext.Setup(x => x.RequestServices.GetService(typeof(IUserService)))
                .Returns(mockUserService.Object);

            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            mockTicketService.Setup(s => s.GetTickets()).Returns(Task.FromResult(tickets));

            var account = await controller.GetTickets();

            var objectResult = account as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        public async Task GetTicketsIfSameUserOkTest()
        {
            var mockHttpContext = new Mock<HttpContext>();
            var mockHeaderHttp = new Mock<IHeaderDictionary>();
            mockHeaderHttp.Setup(x => x[It.IsAny<string>()]).Returns("someHeader");
            var mockHttpRequest = new Mock<HttpRequest>();
            mockHttpRequest.Setup(s => s.Headers).Returns(mockHeaderHttp.Object);
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockHttpContext.Setup(s => s.Request).Returns(mockHttpRequest.Object);
            mockUserService.Setup(s => s.RetrieveUserFromToken(It.IsAny<string>())).Returns(Task.FromResult(tickets[0].Buyer));
            mockHttpContext.Setup(x => x.RequestServices.GetService(typeof(IUserService)))
                .Returns(mockUserService.Object);

            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            mockTicketService.Setup(s => s.GetUserTickets(1)).Returns(Task.FromResult(tickets));

            var account = await controller.GetTickets();

            var objectResult = account as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        public async Task GetTicketOkTest()
        {
            mockTicketService.Setup(s => s.GetTicket(It.IsAny<int>())).Returns(Task.FromResult(tickets[0]));

            var account = await controller.GetTicket(It.IsAny<int>());

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
                        Date = "someDate",
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
                        Date = "someDate",
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