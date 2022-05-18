using System.Collections.Generic;
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
        public void AddPerformerOk()
        {
            var request = new AddPerformerRequest 
            {
                UserInfo = new User { Firstname = "someName"},
                Concerts = new List<Concert>(),
                Genre = 2,
                PerformerType = Constants.PERFORMER_TYPE_SOLO_ARTIST,
                StartYear = "12/03/1998"
            };

            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_SUCCESS,
                Message = "success"
            };
            
            mockService.Setup(s => s.AddPerformer(request)).Returns(operationResult);

            var result = controller.AddPerformer(request);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200,statusCode);
        }

        [TestMethod]
        public void AddPerformerBadRequest()
        {
            var request = new AddPerformerRequest 
            {
                UserInfo = new User { Firstname = "someName"},
                Concerts = new List<Concert>(),
                Genre = 2,
                PerformerType = Constants.PERFORMER_TYPE_SOLO_ARTIST,
                StartYear = "12/03/1998"
            };

            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_FAIL,
                Message = "error"
            };
            
            mockService.Setup(s => s.AddPerformer(request)).Returns(operationResult);

            var result = controller.AddPerformer(request);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(400,statusCode);
        }

        [TestMethod]
        public void UpdatePerfomerOk()
        {
            var request = new UpdatePerformerRequest 
            {
                Id = 2,
                UserInfo = new User { Firstname = "someName"},
                Artists = new List<Performer>(),
                Genre = 2,
                PerformerType = Constants.PERFORMER_TYPE_SOLO_ARTIST,
                StartYear = "12/03/1998"
            };

            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_SUCCESS,
                Message = "success"
            };
            
            mockService.Setup(s => s.UpdatePerformer(request)).Returns(operationResult);

            var result = controller.UpdatePerformer(It.IsAny<int>(),request);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200,statusCode);
        }

        [TestMethod]
        public void UpdatePerfomerBadRequest()
        {
            var request = new UpdatePerformerRequest 
            {
                Id = 2,
                UserInfo = new User {Firstname = "someName" },
                Artists = new List<Performer>(),
                Genre = 2,
                PerformerType = Constants.PERFORMER_TYPE_SOLO_ARTIST,
                StartYear = "12/03/1998"
            };

            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_FAIL,
                Message = "fail"
            };
            
            mockService.Setup(s => s.UpdatePerformer(request)).Returns(operationResult);

            var result = controller.UpdatePerformer(It.IsAny<int>(),request);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(400,statusCode);
        }

        [TestMethod]
        public void DeletePerformerOk()
        {
            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_SUCCESS,
                Message = "success"
            };

            mockService.Setup(s => s.DeletePerformer(It.IsAny<int>())).Returns(operationResult);

            var result = controller.DeletePerformer(It.IsAny<int>());
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200,statusCode);
        }

        [TestMethod]
        public void DeletePerformerBadRequest()
        {
            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_FAIL,
                Message = "fail"
            };

            mockService.Setup(s => s.DeletePerformer(It.IsAny<int>())).Returns(operationResult);

            var result = controller.DeletePerformer(It.IsAny<int>());
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(400,statusCode);
        }

        [TestMethod]
        public void GetPerformerOk()
        {
            mockService.Setup(s => s.GetPerformer(It.IsAny<int>())).Returns(performers[0]);

            var result = controller.GetPerformer(It.IsAny<int>());
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200,statusCode);
        }

        [TestMethod]
        public void GetPerformersOk()
        {
            mockService.Setup(s => s.GetPerformers()).Returns(performers);

            var result = controller.GetPerformers();
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200,statusCode);
        }


        private List<Performer> SetupPerformers()
        {
            return new List<Performer>
            {
                new Performer 
                {
                    Id = 1,
                    Name = "someName",
                    Artists = "name1|name2|name3",
                    Genre = new Genre(),
                    PerformerType = Constants.PERFORMER_TYPE_SOLO_ARTIST,
                    StartYear = "12/03/1998"
                },
                new Performer 
                {
                    Id = 2,
                    Name = "someName",
                    Artists = "name1|name2|name3",
                    Genre = new Genre(),
                    PerformerType = Constants.PERFORMER_TYPE_BAND,
                    StartYear = "12/03/1998"
                },
                new Performer 
                {
                    Id = 3,
                    Name = "someName",
                    Artists = "name1|name2|name3",
                    Genre = new Genre(),
                    PerformerType = Constants.PERFORMER_TYPE_SOLO_ARTIST,
                    StartYear = "10/03/2000"
                },
            };
        }
    }
}