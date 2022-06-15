using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Models.Param;
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
            mockService.Setup(s => s.GetPerformers(It.IsAny<PerformerSearchParam>())).Returns(Task.FromResult(performers));

            var result = await controller.GetPerformers(null);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public async Task GetBandPerformerTypeByNameOk()
        {
            var performer = new Performer()
            {
                Id = 1,
                UserInfo = new User()
                {
                    Id = 1,
                    ActiveAccount = true,
                    Firstname = "Next 5",
                    Lastname = "",
                    Email = "next5@example.com",
                    Password = "3497fhisdubvcqwu2ye",
                    Role = Constants.ROLE_ARTIST,
                    Token = "ruvahbo284IUWGFADsifhawoiefu"
                },
                Genre = new Genre()
                {
                    Id = 1,
                    Name = "Rock"
                },
                Members = new List<Performer>(),
                PerformerType = Constants.PERFORMER_TYPE_BAND,
                StartYear = "2015"
            };
            var request = new PerformerSearchParam()
            {
                PerformerName = performer.UserInfo.Firstname + " " + performer.UserInfo.Lastname
            };

            List<Performer> performers = new List<Performer>();
            performers.Add(performer);

            mockService.Setup(s => s.GetPerformers(request)).Returns(Task.FromResult(performers));

            var result = await controller.GetPerformers(request);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public async Task GetSoloArtistPerformerTypeByNameOk()
        {
            var performer = new Performer()
            {
                Id = 2,
                UserInfo = new User()
                {
                    Id = 2,
                    ActiveAccount = true,
                    Firstname = "William",
                    Lastname = "Olsen",
                    Email = "wolsen@example.com",
                    Password = "sdufhawifeu",
                    Role = Constants.ROLE_ARTIST,
                    Token = "sdkafhaw94t7fhsifuahwiufs"
                },
                Genre = new Genre()
                {
                    Id = 1,
                    Name = "Rock"
                },
                Members = new List<Performer>(),
                PerformerType = Constants.PERFORMER_TYPE_SOLO_ARTIST,
                StartYear = "2015"
            };
            var request = new PerformerSearchParam()
            {
                PerformerName = performer.UserInfo.Firstname + " " + performer.UserInfo.Lastname
            };

            List<Performer> performers = new List<Performer>();
            performers.Add(performer);

            mockService.Setup(s => s.GetPerformers(request)).Returns(Task.FromResult(performers));

            var result = await controller.GetPerformers(request);
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