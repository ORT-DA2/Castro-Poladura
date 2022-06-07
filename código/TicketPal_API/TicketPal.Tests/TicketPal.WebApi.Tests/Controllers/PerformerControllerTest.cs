using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Models.Request;
using TicketPal.Domain.Models.Response;
using TicketPal.Interfaces.Services.Performers;
using TicketPal.WebApi.Controllers;

namespace TicketPal.WebApi.Tests.Controllers
{
    [TestClass]
    public class PerformerControllerTest
    {
        private Mock<IPerformerService> mockService;
        private List<Performer> performers;
        private PerformersController controller;

        [TestInitialize]
        public void TestSetup()
        {
            mockService = new Mock<IPerformerService>(MockBehavior.Default);
            controller = new PerformersController(mockService.Object);
            this.performers = SetupPerformers();
        }

        [TestMethod]
        public async Task AddPerformerOk()
        {
            var request = new AddPerformerRequest
            {
                UserId = 1,
                MembersIds = new List<int>(),
                Genre = 2,
                PerformerType = Constants.PERFORMER_TYPE_SOLO_ARTIST,
                StartYear = "12/03/1998"
            };

            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_SUCCESS,
                Message = "success"
            };

            mockService.Setup(s => s.AddPerformer(request)).Returns(Task.FromResult(operationResult));

            var result = await controller.AddPerformer(request);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public async Task AddPerformerBadRequest()
        {
            var request = new AddPerformerRequest
            {
                UserId = 1,
                MembersIds = new List<int>(),
                Genre = 2,
                PerformerType = Constants.PERFORMER_TYPE_SOLO_ARTIST,
                StartYear = "12/03/1998"
            };

            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_FAIL,
                Message = "error"
            };

            mockService.Setup(s => s.AddPerformer(request)).Returns(Task.FromResult(operationResult));

            var result = await controller.AddPerformer(request);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(400, statusCode);
        }

        [TestMethod]
        public async Task UpdatePerfomerOk()
        {
            var request = new UpdatePerformerRequest
            {
                Id = 2,
                UserId = 1,
                ArtistsIds = new List<int> { 1, 2, 3 },
                GenreId = 2,
                StartYear = "12/03/1998"
            };

            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_SUCCESS,
                Message = "success"
            };

            mockService.Setup(s => s.UpdatePerformer(request)).Returns(Task.FromResult(operationResult));

            var result = await controller.UpdatePerformer(It.IsAny<int>(), request);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public async Task UpdatePerfomerBadRequest()
        {
            var request = new UpdatePerformerRequest
            {
                Id = 2,
                UserId = 2,
                ArtistsIds = new List<int> { 1, 2 },
                GenreId = 2,
                StartYear = "12/03/1998"
            };

            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_FAIL,
                Message = "fail"
            };

            mockService.Setup(s => s.UpdatePerformer(request)).Returns(Task.FromResult(operationResult));

            var result = await controller.UpdatePerformer(It.IsAny<int>(), request);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(400, statusCode);
        }

        [TestMethod]
        public async Task DeletePerformerOk()
        {
            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_SUCCESS,
                Message = "success"
            };

            mockService.Setup(s => s.DeletePerformer(It.IsAny<int>())).Returns(Task.FromResult(operationResult));

            var result = await controller.DeletePerformer(It.IsAny<int>());
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public async Task DeletePerformerBadRequest()
        {
            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_FAIL,
                Message = "fail"
            };

            mockService.Setup(s => s.DeletePerformer(It.IsAny<int>())).Returns(Task.FromResult(operationResult));

            var result = await controller.DeletePerformer(It.IsAny<int>());
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(400, statusCode);
        }

        [TestMethod]
        public async Task GetPerformerOk()
        {
            mockService.Setup(s => s.GetPerformer(It.IsAny<int>())).Returns(Task.FromResult(performers[0]));

            var result = await controller.GetPerformer(It.IsAny<int>());
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public async Task GetPerformersOk()
        {
            mockService.Setup(s => s.GetPerformers()).Returns(Task.FromResult(performers));

            var result = await controller.GetPerformers();
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }


        private List<Performer> SetupPerformers()
        {
            return new List<Performer>
            {
                new Performer
                {
                    Id = 1,
                    UserInfo = new User { Firstname = "someName"},
                    Members = new List<Performer>(),
                    Genre = new Genre(),
                    PerformerType = Constants.PERFORMER_TYPE_SOLO_ARTIST,
                    StartYear = "12/03/1998"
                },
                new Performer
                {
                    Id = 2,
                    UserInfo = new User { Firstname = "someName"},
                    Members = new List<Performer>(),
                    Genre = new Genre(),
                    PerformerType = Constants.PERFORMER_TYPE_BAND,
                    StartYear = "12/03/1998"
                },
                new Performer
                {
                    Id = 3,
                    UserInfo = new User { Firstname = "someName" },
                    Members = new List<Performer>(),
                    Genre = new Genre(),
                    PerformerType = Constants.PERFORMER_TYPE_SOLO_ARTIST,
                    StartYear = "10/03/2000"
                },
            };
        }
    }
}