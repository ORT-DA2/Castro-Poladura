using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Models.Request;
using TicketPal.Domain.Models.Response;
using TicketPal.Interfaces.Services.Genres;
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
        public async Task RegisterGenreOk()
        {
            var request = new AddGenreRequest
            {
                Name = "someName",
            };

            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_SUCCESS,
                Message = "success"
            };

            mockService.Setup(s => s.AddGenre(request)).Returns(Task.FromResult(operationResult));

            var result = await controller.AddGenre(request);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public async Task RegisterGenreBadRequest()
        {
            var request = new AddGenreRequest
            {
                Name = "someName",
            };

            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_FAIL,
                Message = "fail"
            };

            mockService.Setup(s => s.AddGenre(request)).Returns(Task.FromResult(operationResult));

            var result = await controller.AddGenre(request);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(400, statusCode);
        }

        [TestMethod]
        public async Task DeleteGenreOk()
        {
            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_SUCCESS,
                Message = "success"
            };

            mockService.Setup(s => s.DeleteGenre(It.IsAny<int>())).Returns(Task.FromResult(operationResult));

            var result = await controller.DeleteGenre(It.IsAny<int>());
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public async Task DeleteGenreBadRequest()
        {
            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_FAIL,
                Message = "error"
            };

            mockService.Setup(s => s.DeleteGenre(It.IsAny<int>())).Returns(Task.FromResult(operationResult));

            var result = await controller.DeleteGenre(It.IsAny<int>());
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(400, statusCode);
        }

        [TestMethod]
        public async Task GetGenreOk()
        {
            mockService.Setup(s => s.GetGenre(It.IsAny<int>())).Returns(Task.FromResult(genres[0]));

            var result = await controller.GetGenre(It.IsAny<int>());
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public async Task GetGenresOk()
        {
            mockService.Setup(s => s.GetGenres()).Returns(Task.FromResult(genres));

            var result = await controller.GetGenres();
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
                Name = "newName"
            };

            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_SUCCESS,
                Message = "success"
            };

            mockService.Setup(s => s.UpdateGenre(request)).Returns(operationResult);

            var result = controller.UpdateGenre(It.IsAny<int>(), request);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public void UpdateGenreBadRequest()
        {
            var request = new UpdateGenreRequest
            {
                Id = 2,
                Name = "newName"
            };

            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_FAIL,
                Message = "fail"
            };

            mockService.Setup(s => s.UpdateGenre(request)).Returns(operationResult);

            var result = controller.UpdateGenre(It.IsAny<int>(), request);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(400, statusCode);
        }

        private List<Genre> SetupGenres()
        {
            return new List<Genre>
            {
                new Genre
                {
                    Id = 1,
                    Name = "Pop"
                },
                new Genre
                {
                    Id = 2,
                    Name = "Rock"
                },
                new Genre
                {
                    Id = 3,
                    Name = "Soul"
                },
                new Genre
                {
                    Id = 4,
                    Name = "Music"
                },
            };
        }
    }
}