using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using TicketPal.BusinessLogic.Services.Genres;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Exceptions;
using TicketPal.Domain.Models.Request;
using TicketPal.Domain.Models.Response;

namespace TicketPal.BusinessLogic.Tests.Services.Genres
{
    [TestClass]
    public class GenreServiceTests : BaseServiceTests
    {
        private GenreEntity genre;
        private AddGenreRequest genreRequest;
        private GenreService genreService;

        [TestInitialize]
        public void Initialize()
        {
            genre = new GenreEntity()
            {
                Id = 1,
                GenreName = "Rock"
            };

            genreRequest = new AddGenreRequest()
            {
                GenreName = genre.GenreName
            };

            this.genresMock.Setup(m => m.Exists(It.IsAny<int>())).Returns(false);
            this.genresMock.Setup(m => m.Add(It.IsAny<GenreEntity>())).Verifiable();

            genreService = new GenreService(this.genresMock.Object, this.testAppSettings, this.mapper);
        }

        [TestMethod]
        public void AddGenreSuccesfullyTest()
        {
            OperationResult result = genreService.AddGenre(genreRequest);

            Assert.IsTrue(result.ResultCode == ResultCode.SUCCESS);
        }

        [TestMethod]
        public void AddGenreTwiceFailsTest()
        {
            genreService.AddGenre(genreRequest);

            this.genresMock.Setup(m => m.Exists(It.IsAny<int>())).Returns(true);
            this.genresMock.Setup(m => m.Add(It.IsAny<GenreEntity>())).Throws(new RepositoryException());
            genreService = new GenreService(this.genresMock.Object, this.testAppSettings, this.mapper);

            OperationResult result = genreService.AddGenre(genreRequest);

            Assert.IsTrue(result.ResultCode == ResultCode.FAIL);
        }

        [TestMethod]
        public void DeleteGenreSuccesfullyTest()
        {
            var id = 1;
            var dbUser = new GenreEntity
            {
                Id = id,
                GenreName = genre.GenreName
            };

            this.genresMock.Setup(m => m.Get(It.IsAny<int>())).Returns(dbUser);
            OperationResult result = genreService.DeleteGenre(id);

            Assert.IsTrue(result.ResultCode == ResultCode.SUCCESS);
        }

        [TestMethod]
        public void DeleteUnexistentGenreFailsTest()
        {
            var id = 1;

            this.genresMock.Setup(m => m.Delete(It.IsAny<int>())).Throws(new RepositoryException());
            this.genreService = new GenreService(this.genresMock.Object, this.testAppSettings, this.mapper);
            OperationResult result = genreService.DeleteGenre(id);

            Assert.IsTrue(result.ResultCode == ResultCode.FAIL);
        }

        [TestMethod]
        public void UpdateGenreSuccesfullyTest()
        {
            var genreName = "Salsa";
            var updateRequest = new UpdateGenreRequest
            {
                GenreName = genreName
            };

            this.genresMock.Setup(m => m.Update(It.IsAny<GenreEntity>())).Verifiable();
            OperationResult expected = genreService.UpdateGenre(updateRequest);

            Assert.IsTrue(expected.ResultCode == ResultCode.SUCCESS);
        }

        [TestMethod]
        public void GetUserByIdTest()
        {
            int id = 1;
            var dbUser = new GenreEntity
            {
                Id = id,
                GenreName = genre.GenreName
            };

            this.genresMock.Setup(r => r.Get(It.IsAny<int>())).Returns(dbUser);
            this.genreService = new GenreService(this.genresMock.Object, this.testAppSettings, this.mapper);
            Genre concert = genreService.GetGenre(id);

            Assert.IsNotNull(concert);
            Assert.IsTrue(id == concert.Id);
        }

        [TestMethod]
        public void GetGenreByNullIdTest()
        {
            int id = 1;

            GenreEntity dbUser = null;

            this.genresMock.Setup(r => r.Get(It.IsAny<int>())).Returns(dbUser);
            this.genreService = new GenreService(this.genresMock.Object, this.testAppSettings, this.mapper);
            Genre concert = genreService.GetGenre(id);

            Assert.IsNull(concert);

        }

        [TestMethod]
        public void GetAllGenresSuccesfullyTest()
        {
            IEnumerable<GenreEntity> dbAccounts = new List<GenreEntity>()
            {
                new GenreEntity
                {
                    Id = 1,
                    GenreName = genre.GenreName
                },
                new GenreEntity
                {
                    Id = 2,
                    GenreName = "Pop"
                },
                new GenreEntity
                {
                    Id = 3,
                    GenreName = "Blues"
                },
            };

            this.genresMock.Setup(r => r.GetAll()).Returns(dbAccounts);
            this.genreService = new GenreService(this.genresMock.Object, this.testAppSettings, this.mapper);
            IEnumerable<Genre> result = genreService.GetGenres();

            Assert.IsTrue(result.ToList().Count == 3);
        }
    }
}
