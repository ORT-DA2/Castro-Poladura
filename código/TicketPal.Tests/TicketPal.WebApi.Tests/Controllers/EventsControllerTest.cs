using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Entity;
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
        public void AddEvent()
        {
            var request = new AddConcertRequest
            {
                Artist = 2,
                Date = DateTime.Now,
                AvailableTickets = 201,
                EventType = EventType.CONCERT,
                TicketPrice = 197.8M,
                CurrencyType = Domain.Constants.CurrencyType.USD,
                TourName = "SomeTour"
            };

            var operationResult = new OperationResult
            {
                ResultCode = ResultCode.SUCCESS,
                Message = "success"
            };

            mockService.Setup(s => s.AddConcert(request)).Returns(operationResult);

            var result = controller.AddConcert(request);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public void AddEventBadRequest()
        {
            var request = new AddConcertRequest
            {
                Artist = 2,
                Date = DateTime.Now,
                AvailableTickets = 201,
                EventType = EventType.CONCERT,
                TicketPrice = 197.8M,
                CurrencyType = Domain.Constants.CurrencyType.USD,
                TourName = "SomeTour"
            };

            var operationResult = new OperationResult
            {
                ResultCode = ResultCode.FAIL,
                Message = "error"
            };

            mockService.Setup(s => s.AddConcert(request)).Returns(operationResult);

            var result = controller.AddConcert(request);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(400, statusCode);
        }

        [TestMethod]
        public void UpdateEventOk()
        {
            var request = new UpdateConcertRequest
            {
                Id = 1,
                Artist = 2,
                Date = DateTime.Now,
                AvailableTickets = 201,
                EventType = EventType.CONCERT,
                TicketPrice = 197.8M,
                CurrencyType = Domain.Constants.CurrencyType.USD,
                TourName = "SomeTour"
            };

            var operationResult = new OperationResult
            {
                ResultCode = ResultCode.SUCCESS,
                Message = "success"
            };

            mockService.Setup(s => s.UpdateConcert(request)).Returns(operationResult);

            var result = controller.UpdateConcert(It.IsAny<int>(), request);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public void UpdateEventBadRequest()
        {
            var request = new UpdateConcertRequest
            {
                Id = 1,
                Artist = 2,
                Date = DateTime.Now,
                AvailableTickets = 201,
                EventType = EventType.CONCERT,
                TicketPrice = 197.8M,
                CurrencyType = Domain.Constants.CurrencyType.USD,
                TourName = "SomeTour"
            };

            var operationResult = new OperationResult
            {
                ResultCode = ResultCode.FAIL,
                Message = "error"
            };

            mockService.Setup(s => s.UpdateConcert(request)).Returns(operationResult);

            var result = controller.UpdateConcert(It.IsAny<int>(), request);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(400, statusCode);
        }

        [TestMethod]
        public void DeleteEventOk()
        {

            var operationResult = new OperationResult
            {
                ResultCode = ResultCode.SUCCESS,
                Message = "success"
            };

            mockService.Setup(s => s.DeleteConcert(It.IsAny<int>())).Returns(operationResult);

            var result = controller.DeleteConcert(It.IsAny<int>());
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public void DeleteEventBadRequest()
        {

            var operationResult = new OperationResult
            {
                ResultCode = ResultCode.FAIL,
                Message = "error"
            };

            mockService.Setup(s => s.DeleteConcert(It.IsAny<int>())).Returns(operationResult);

            var result = controller.DeleteConcert(It.IsAny<int>());
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(400, statusCode);
        }

        [TestMethod]
        public void GetEventOk()
        {
            mockService.Setup(s => s.GetConcert(It.IsAny<int>())).Returns(concerts[0]);

            var result = controller.GetConcert(It.IsAny<int>());
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public void GetConcerts()
        {
            mockService.Setup(s => s.GetConcerts(
                It.IsAny<EventType>(),
                true,
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()
            )).Returns(concerts);

            var result = controller.GetConcerts(
                EventType.CONCERT,
                true,
                DateTime.Now.ToString("dd/M/yyyy"),
                DateTime.Now.AddDays(30).ToString("dd/M/yyyy"),
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
                EventType.CONCERT,
                false,
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()
            )).Returns(concerts);

            var result = controller.GetConcerts(
                0,
                true,
                "3fewfsdd",
                DateTime.Now.AddDays(30).ToString("dd/M/yyyy"),
                "Bono"
                );
            var operationResult = new OperationResult
            {
                ResultCode = ResultCode.FAIL,
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
                EventType.CONCERT,
                false,
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()
            )).Returns(concerts);

            var result = controller.GetConcerts(
                0,
                true,
                DateTime.Now.AddDays(30).ToString("dd/M/yyyy"),
                "fdsf23fs",
                "Bono"
                );
            var operationResult = new OperationResult
            {
                ResultCode = ResultCode.FAIL,
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
                    Artist = new Performer(),
                    Date = DateTime.Now,
                    AvailableTickets = 201,
                    EventType = EventType.CONCERT,
                    TicketPrice = 197.8M,
                    CurrencyType = Domain.Constants.CurrencyType.USD,
                    TourName = "SomeTour"
                },
                new Concert
                {
                    Id = 2,
                    Artist = new Performer(),
                    Date = DateTime.Now,
                    AvailableTickets = 201,
                    EventType = EventType.CONCERT,
                    TicketPrice = 197.8M,
                    CurrencyType = Domain.Constants.CurrencyType.USD,
                    TourName = "SomeTour"
                },
                new Concert
                {
                    Id = 3,
                    Artist = new Performer(),
                    Date = DateTime.Now,
                    AvailableTickets = 201,
                    EventType = EventType.CONCERT,
                    TicketPrice = 197.8M,
                    CurrencyType = Domain.Constants.CurrencyType.USD,
                    TourName = "SomeTour"
                },
            };
        }

    }
}