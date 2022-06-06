using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TicketPal.Domain.Constants;
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
        public void GetEventOk()
        {
            mockService.Setup(s => s.GetConcert(It.IsAny<int>())).Returns(Task.FromResult(concerts[0]));

            var result = controller.GetConcert(It.IsAny<int>());
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public void GetConcerts()
        {
            mockService.Setup(s => s.GetConcerts(
                It.IsAny<string>(),
                true,
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()
            )).Returns(Task.FromResult(concerts));

            var result = controller.GetConcerts(
                Constants.EVENT_CONCERT_TYPE,
                true,
                DateTime.Now.ToString("dd/M/yyyy hh:mm"),
                DateTime.Now.AddDays(30).ToString("dd/M/yyyy hh:mm"),
                "Bono"
                );
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);

        }

        [TestMethod]
        public void GetConcertsWrongStartDate()
        {
            mockService.Setup(s => s.GetConcerts(
                Constants.EVENT_CONCERT_TYPE,
                false,
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()
            )).Returns(Task.FromResult(concerts));

            var result = controller.GetConcerts(
                Constants.EVENT_CONCERT_TYPE,
                true,
                "3fewfsdd",
                DateTime.Now.AddDays(30).ToString("dd/M/yyyy"),
                "Bono"
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
        public void GetConcertsWrongEndDate()
        {
            mockService.Setup(s => s.GetConcerts(
                Constants.EVENT_CONCERT_TYPE,
                false,
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()
            )).Returns(Task.FromResult(concerts));

            var result = controller.GetConcerts(
                Constants.EVENT_CONCERT_TYPE,
                true,
                DateTime.Now.AddDays(30).ToString("dd/M/yyyy"),
                "fdsf23fs",
                "Bono"
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
                    Date = DateTime.Now,
                    AvailableTickets = 201,
                    EventType = Constants.EVENT_CONCERT_TYPE,
                    TicketPrice = 197.8M,
                    CurrencyType = Constants.CURRENCY_US_DOLLARS,
                    TourName = "SomeTour"
                },
                new Concert
                {
                    Id = 2,
                    Date = DateTime.Now,
                    AvailableTickets = 201,
                    EventType = Constants.EVENT_CONCERT_TYPE,
                    TicketPrice = 197.8M,
                    CurrencyType = Constants.CURRENCY_US_DOLLARS,
                    TourName = "SomeTour"
                },
                new Concert
                {
                    Id = 3,
                    Date = DateTime.Now,
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