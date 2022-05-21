﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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

            this.mockGenreRepo.Setup(m => m.Exists(It.IsAny<int>())).Returns(false);
            this.mockGenreRepo.Setup(m => m.Add(It.IsAny<GenreEntity>())).Verifiable();

            this.factoryMock.Setup(m => m.GetRepository(typeof(GenreEntity))).Returns(this.mockGenreRepo.Object);

            this.genreService = new GenreService(this.factoryMock.Object, this.mapper);
        }

        [TestMethod]
        public void AddGenreSuccesfullyTest()
        {
            OperationResult result = genreService.AddGenre(genreRequest);

            Assert.IsTrue(result.ResultCode == Constants.CODE_SUCCESS);
        }

        [TestMethod]
        public void AddGenreTwiceFailsTest()
        {
            genreService.AddGenre(genreRequest);

            this.mockGenreRepo.Setup(m => m.Exists(It.IsAny<int>())).Returns(true);
            this.mockGenreRepo.Setup(m => m.Add(It.IsAny<GenreEntity>())).Throws(new RepositoryException());
            this.factoryMock.Setup(m => m.GetRepository(typeof(GenreEntity))).Returns(this.mockGenreRepo.Object);

            this.genreService = new GenreService(this.factoryMock.Object, this.mapper);

            OperationResult result = genreService.AddGenre(genreRequest);

            Assert.IsTrue(result.ResultCode == Constants.CODE_FAIL);
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

            this.mockGenreRepo.Setup(m => m.Get(It.IsAny<int>())).Returns(dbUser);
            this.factoryMock.Setup(m => m.GetRepository(typeof(GenreEntity))).Returns(this.mockGenreRepo.Object);

            this.genreService = new GenreService(this.factoryMock.Object, this.mapper);
            OperationResult result = genreService.DeleteGenre(id);

            Assert.IsTrue(result.ResultCode == Constants.CODE_SUCCESS);
        }

        [TestMethod]
        public void DeleteUnexistentGenreFailsTest()
        {
            var id = 1;

            this.mockGenreRepo.Setup(m => m.Delete(It.IsAny<int>())).Throws(new RepositoryException());
            this.factoryMock.Setup(m => m.GetRepository(typeof(GenreEntity))).Returns(this.mockGenreRepo.Object);

            this.genreService = new GenreService(this.factoryMock.Object, this.mapper);
            OperationResult result = genreService.DeleteGenre(id);

            Assert.IsTrue(result.ResultCode == Constants.CODE_FAIL);
        }

        [TestMethod]
        public void UpdateGenreSuccesfullyTest()
        {
            var genreName = "Salsa";
            var updateRequest = new UpdateGenreRequest
            {
                GenreName = genreName
            };

            this.mockGenreRepo.Setup(m => m.Update(It.IsAny<GenreEntity>())).Verifiable();
            this.factoryMock.Setup(m => m.GetRepository(typeof(GenreEntity))).Returns(this.mockGenreRepo.Object);

            this.genreService = new GenreService(this.factoryMock.Object, this.mapper);
            OperationResult expected = genreService.UpdateGenre(updateRequest);

            Assert.IsTrue(expected.ResultCode == Constants.CODE_SUCCESS);
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

            this.mockGenreRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(dbUser);
            this.factoryMock.Setup(m => m.GetRepository(typeof(GenreEntity))).Returns(this.mockGenreRepo.Object);

            this.genreService = new GenreService(this.factoryMock.Object, this.mapper);
            Genre concert = genreService.GetGenre(id);

            Assert.IsNotNull(concert);
            Assert.IsTrue(id == concert.Id);
        }

        [TestMethod]
        public void GetGenreByNullIdTest()
        {
            int id = 1;

            GenreEntity dbUser = null;

            this.mockGenreRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(dbUser);
            this.factoryMock.Setup(m => m.GetRepository(typeof(GenreEntity))).Returns(this.mockGenreRepo.Object);

            this.genreService = new GenreService(this.factoryMock.Object, this.mapper);
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

            this.mockGenreRepo.Setup(r => r.GetAll()).Returns(dbAccounts);
            this.factoryMock.Setup(m => m.GetRepository(typeof(GenreEntity))).Returns(this.mockGenreRepo.Object);

            this.genreService = new GenreService(this.factoryMock.Object, this.mapper);
            IEnumerable<Genre> result = genreService.GetGenres();

            Assert.IsTrue(result.ToList().Count == 3);
        }
    }
}
