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
using TicketPal.Interfaces.Services.Genres;
using TicketPal.Interfaces.Services.Users;
using TicketPal.WebApi.Controllers;

namespace TicketPal.WebApi.Tests.Controllers
{
    
    [TestClass]
    public class GenreControllerTest
    {
        private Mock<IGenreService> mockService;
        private List<Genre> genres;
        private GenresController controller;

        [TestInitialize]
        public void TestSetup()
        {
            mockService = new Mock<IGenreService>(MockBehavior.Default);
            controller = new GenresController(mockService.Object);
            this.genres = SetupGenres();
        }

        [TestMethod]
        public void RegisterGenreOk()
        {
            var request = new AddGenreRequest
            {
                GenreName = "someName",
            };

            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_SUCCESS,
                Message = "success"
            };

            mockService.Setup(s => s.AddGenre(request)).Returns(operationResult);

            var result = controller.AddGenre(request);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);   
        }

        [TestMethod]
        public void RegisterGenreBadRequest()
        {
            var request = new AddGenreRequest
            {
                GenreName = "someName",
            };

            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_FAIL,
                Message = "fail"
            };

            mockService.Setup(s => s.AddGenre(request)).Returns(operationResult);

            var result = controller.AddGenre(request);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(400, statusCode);   
        }

        [TestMethod]
        public void DeleteGenreOk()
        {
            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_SUCCESS,
                Message = "success"
            };

            mockService.Setup(s => s.DeleteGenre(It.IsAny<int>())).Returns(operationResult);

            var result = controller.DeleteGenre(It.IsAny<int>());
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);   
        }

        [TestMethod]
        public void DeleteGenreBadRequest()
        {
            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_FAIL,
                Message = "error"
            };

            mockService.Setup(s => s.DeleteGenre(It.IsAny<int>())).Returns(operationResult);

            var result = controller.DeleteGenre(It.IsAny<int>());
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(400, statusCode);   
        }

        [TestMethod]
        public void GetGenreOk()
        {
            mockService.Setup(s => s.GetGenre(It.IsAny<int>())).Returns(genres[0]);
            
            var result = controller.GetGenre(It.IsAny<int>());
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public void GetGenresOk()
        {
            mockService.Setup(s => s.GetGenres()).Returns(genres);
            
            var result = controller.GetGenres();
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public void UpdateGenreOk()
        {   
            var request = new UpdateGenreRequest 
            {   
                Id = 2,
                GenreName = "newName"
            };

            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_SUCCESS,
                Message = "success"
            };

            mockService.Setup(s => s.UpdateGenre(request)).Returns(operationResult);
            
            var result = controller.UpdateGenre(It.IsAny<int>(),request);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200,statusCode);
        }

        [TestMethod]
        public void UpdateGenreBadRequest()
        {   
            var request = new UpdateGenreRequest 
            {   
                Id = 2,
                GenreName = "newName"
            };

            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_FAIL,
                Message = "fail"
            };

            mockService.Setup(s => s.UpdateGenre(request)).Returns(operationResult);
            
            var result = controller.UpdateGenre(It.IsAny<int>(),request);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(400,statusCode);
        }

        private List<Genre> SetupGenres()
        {
            return new List<Genre>
            {
                new Genre 
                {
                    Id = 1,
                    GenreName = "Pop"
                },
                new Genre 
                {
                    Id = 2,
                    GenreName = "Rock"
                },
                new Genre 
                {
                    Id = 3,
                    GenreName = "Soul"
                },
                new Genre 
                {
                    Id = 4,
                    GenreName = "Music"
                },
            };
        }
    }
}