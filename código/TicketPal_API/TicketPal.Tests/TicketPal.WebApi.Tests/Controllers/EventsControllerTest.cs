using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Models.Param;
using TicketPal.Domain.Models.Request;
using TicketPal.Domain.Models.Response;
using TicketPal.Interfaces.Services.Concerts;
using TicketPal.WebApi.Controllers;

namespace TicketPal.WebApi.Tests.Controllers
{
    [TestClass]
    public class EventsControllerTest
    {
        private Mock<IConcertService> mockService;
        private List<Concert> concerts;
        private EventsController controller;

        [TestInitialize]
        public void TestSetup()
        {
            mockService = new Mock<IConcertService>(MockBehavior.Default);
            controller = new EventsController(mockService.Object);
            this.concerts = SetupEvents();
        }

        [TestMethod]
        public async Task AddEvent()
        {
            var request = new AddConcertRequest
            {
                ArtistsIds = new List<int> { 2 },
                Date = DateTime.Now,
                AvailableTickets = 201,
                EventType = Constants.EVENT_CONCERT_TYPE,
                TicketPrice = 197.8M,
                CurrencyType = Constants.CURRENCY_US_DOLLARS,
                TourName = "SomeTour"
            };

            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_SUCCESS,
                Message = "success"
            };

            mockService.Setup(s => s.AddConcert(request)).Returns(Task.FromResult(operationResult));

            var result = await controller.AddConcert(request);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public async Task AddEventBadRequest()
        {
            var request = new AddConcertRequest
            {
                ArtistsIds = new List<int> { 2 },
                Date = DateTime.Now,
                AvailableTickets = 201,
                EventType = Constants.EVENT_CONCERT_TYPE,
                TicketPrice = 197.8M,
                CurrencyType = Constants.CURRENCY_US_DOLLARS,
                TourName = "SomeTour"
            };

            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_FAIL,
                Message = "error"
            };

            mockService.Setup(s => s.AddConcert(request)).Returns(Task.FromResult(operationResult));

            var result = await controller.AddConcert(request);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(400, statusCode);
        }

        [TestMethod]
        public async Task UpdateEventOk()
        {
            var request = new UpdateConcertRequest
            {
                Id = 1,
                Date = DateTime.Now,
                EventType = Constants.EVENT_CONCERT_TYPE,
                TicketPrice = 197.8M,
                CurrencyType = Constants.CURRENCY_US_DOLLARS,
                TourName = "SomeTour"
            };

            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_SUCCESS,
                Message = "success"
            };

            mockService.Setup(s => s.UpdateConcert(request)).Returns(Task.FromResult(operationResult));

            var result = await controller.UpdateConcert(It.IsAny<int>(), request);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public async Task UpdateEventBadRequest()
        {
            var request = new UpdateConcertRequest
            {
                Id = 1,
                Date = DateTime.Now,
                EventType = Constants.EVENT_CONCERT_TYPE,
                TicketPrice = 197.8M,
                CurrencyType = Constants.CURRENCY_US_DOLLARS,
                TourName = "SomeTour"
            };

            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_FAIL,
                Message = "error"
            };

            mockService.Setup(s => s.UpdateConcert(request)).Returns(Task.FromResult(operationResult));

            var result = await controller.UpdateConcert(It.IsAny<int>(), request);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(400, statusCode);
        }

        [TestMethod]
        public async Task DeleteEventOk()
        {

            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_SUCCESS,
                Message = "success"
            };

            mockService.Setup(s => s.DeleteConcert(It.IsAny<int>())).Returns(Task.FromResult(operationResult));

            var result = await controller.DeleteConcert(It.IsAny<int>());
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public async Task DeleteEventBadRequest()
        {

            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_FAIL,
                Message = "error"
            };

            mockService.Setup(s => s.DeleteConcert(It.IsAny<int>())).Returns(Task.FromResult(operationResult));

            var result = await controller.DeleteConcert(It.IsAny<int>());
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(400, statusCode);
        }

        [TestMethod]
        public async Task GetEventOk()
        {
            mockService.Setup(s => s.GetConcert(It.IsAny<int>())).Returns(Task.FromResult(concerts[0]));

            var result = await controller.GetConcert(It.IsAny<int>());
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public async Task GetConcerts()
        {
            mockService.Setup(s => s.GetConcerts(
                new ConcertSearchParam
                {
                    Type = It.IsAny<string>(),
                    Newest = true,
                    StartDate = It.IsAny<string>(),
                    EndDate = It.IsAny<string>(),
                    ArtistName = It.IsAny<string>(),
                    TourName = It.IsAny<string>()
                }
            )).Returns(Task.FromResult(concerts));

            var result = await controller.GetConcerts(
                new ConcertSearchParam
                {
                    Type = Constants.EVENT_CONCERT_TYPE,
                    Newest = true,
                    StartDate = DateTime.Now.ToString("dd/M/yyyy hh:mm"),
                    EndDate = DateTime.Now.AddDays(30).ToString("dd/M/yyyy hh:mm"),
                    ArtistName = "Bono",
                    TourName = "SomeTour"
                });
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);

        }

        [TestMethod]
        public async Task GetConcertsWrongStartDate()
        {
            mockService.Setup(s => s.GetConcerts(
                new ConcertSearchParam
                {
                    Type = It.IsAny<string>(),
                    Newest = true,
                    StartDate = It.IsAny<string>(),
                    EndDate = It.IsAny<string>(),
                    ArtistName = It.IsAny<string>(),
                    TourName = It.IsAny<string>()
                }
            )).Returns(Task.FromResult(concerts));

            var result = await controller.GetConcerts(
                new ConcertSearchParam
                {
                    Type = Constants.EVENT_CONCERT_TYPE,
                    Newest = true,
                    StartDate = "3fewfsdd",
                    EndDate = DateTime.Now.AddDays(30).ToString("dd/M/yyyy"),
                    ArtistName = "Bono",
                    TourName = "SomeTour"
                }
            );
            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_FAIL,
                Message = "error"
            };

            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(400, statusCode);
        }

        [TestMethod]
        public async Task GetConcertsWrongEndDate()
        {
            mockService.Setup(s => s.GetConcerts(
                new ConcertSearchParam
                {
                    Type = It.IsAny<string>(),
                    Newest = true,
                    StartDate = It.IsAny<string>(),
                    EndDate = It.IsAny<string>(),
                    ArtistName = It.IsAny<string>(),
                    TourName = It.IsAny<string>()
                }
            )).Returns(Task.FromResult(concerts));

            var result = await controller.GetConcerts(
                new ConcertSearchParam
                {
                    Type = Constants.EVENT_CONCERT_TYPE,
                    Newest = true,
                    StartDate = DateTime.Now.AddDays(30).ToString("dd/M/yyyy"),
                    EndDate = "fdsf23fs",
                    ArtistName = "Bono",
                    TourName = "SomeTour"
                }
            );
            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_FAIL,
                Message = "error"
            };

            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(400, statusCode);
        }

        private List<Concert> SetupEvents()
        {
            return new List<Concert>
            {
                new Concert
                {
                    Id = 1,
                    Date = "someDate",
                    AvailableTickets = 201,
                    EventType = Constants.EVENT_CONCERT_TYPE,
                    TicketPrice = 197.8M,
                    CurrencyType = Constants.CURRENCY_US_DOLLARS,
                    TourName = "SomeTour"
                },
                new Concert
                {
                    Id = 2,
                    Date = "someDate",
                    AvailableTickets = 201,
                    EventType = Constants.EVENT_CONCERT_TYPE,
                    TicketPrice = 197.8M,
                    CurrencyType = Constants.CURRENCY_US_DOLLARS,
                    TourName = "SomeTour"
                },
                new Concert
                {
                    Id = 3,
                    Date = "someDate",
                    AvailableTickets = 201,
                    EventType = Constants.EVENT_CONCERT_TYPE,
                    TicketPrice = 197.8M,
                    CurrencyType = Constants.CURRENCY_US_DOLLARS,
                    TourName = "SomeTour"
                },
            };
        }

    }
}