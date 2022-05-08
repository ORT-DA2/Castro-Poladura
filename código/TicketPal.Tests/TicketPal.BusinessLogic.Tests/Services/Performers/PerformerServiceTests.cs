using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using TicketPal.BusinessLogic.Services.Performers;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Exceptions;
using TicketPal.Domain.Models.Request;
using TicketPal.Domain.Models.Response;

namespace TicketPal.BusinessLogic.Tests.Services.Performers
{
    [TestClass]
    public class PerformerServiceTests : BaseServiceTests
    {
        private GenreEntity genre;
        private string artists;
        private PerformerEntity performer;
        private AddPerformerRequest performerRequest;
        private PerformerService performerService;

        [TestInitialize]
        public void Initialize()
        {
            genre = new GenreEntity()
            {
                Id = 1,
                GenreName = "Pop Rock"
            };

            artists = "Chris Martin|Jon Buckland|Guy Berryman|Will Champion";

            performer = new PerformerEntity()
            {
                Id = 1,
                Name = "Coldplay",
                PerformerType = Domain.Constants.PerformerType.BAND,
                StartYear = "1996",
                Genre = genre,
                Artists = artists
            };

            performerRequest = new AddPerformerRequest()
            {
                Artists = performer.Artists,
                Genre = genre.Id,
                Name = performer.Name,
                PerformerType = performer.PerformerType,
                StartYear = performer.StartYear
            };

            this.mockPerformerRepo.Setup(m => m.Exists(It.IsAny<int>())).Returns(false);
            this.mockPerformerRepo.Setup(m => m.Add(It.IsAny<PerformerEntity>())).Verifiable();

            this.mockGenreRepo.Setup(m => m.Get(It.IsAny<int>())).Returns(genre);

            this.factoryMock.Setup(m => m.GetRepository(typeof(PerformerEntity))).Returns(this.mockPerformerRepo.Object);
            this.factoryMock.Setup(m => m.GetRepository(typeof(GenreEntity))).Returns(this.mockGenreRepo.Object);

            this.performerService = new PerformerService(this.factoryMock.Object, this.mapper);
        }

        [TestMethod]
        public void AddPerformerSuccesfullyTest()
        {
            OperationResult result = performerService.AddPerformer(performerRequest);

            Assert.IsTrue(result.ResultCode == ResultCode.SUCCESS);
        }

        [TestMethod]
        public void AddPerformerTwiceFailsTest()
        {
            performerService.AddPerformer(performerRequest);

            this.mockPerformerRepo.Setup(m => m.Exists(It.IsAny<int>())).Returns(true);
            this.mockPerformerRepo.Setup(m => m.Add(It.IsAny<PerformerEntity>())).Throws(new RepositoryException());
            this.factoryMock.Setup(m => m.GetRepository(typeof(PerformerEntity))).Returns(this.mockPerformerRepo.Object);

            this.performerService = new PerformerService(this.factoryMock.Object, this.mapper);

            OperationResult result = performerService.AddPerformer(performerRequest);

            Assert.IsTrue(result.ResultCode == ResultCode.FAIL);
        }

        [TestMethod]
        public void AddPerformerWithNoExistentGenreTest()
        {
            this.mockGenreRepo.Setup(m => m.Get(It.IsAny<int>()));

            this.factoryMock.Setup(m => m.GetRepository(typeof(PerformerEntity))).Returns(this.mockPerformerRepo.Object);
            this.factoryMock.Setup(m => m.GetRepository(typeof(GenreEntity))).Returns(this.mockGenreRepo.Object);

            this.performerService = new PerformerService(this.factoryMock.Object, this.mapper);

            OperationResult result = performerService.AddPerformer(performerRequest);

            Assert.IsTrue(result.ResultCode == ResultCode.FAIL);
        }

        [TestMethod]
        public void DeletePerformerSuccesfullyTest()
        {
            var id = 1;
            var dbUser = new PerformerEntity
            {
                Id = id,
                Name = performer.Name,
                Artists = performer.Artists,
                Genre = performer.Genre,
                PerformerType = performer.PerformerType,
                StartYear = performer.StartYear
            };

            this.mockPerformerRepo.Setup(m => m.Get(It.IsAny<int>())).Returns(dbUser);
            this.factoryMock.Setup(m => m.GetRepository(typeof(PerformerEntity))).Returns(this.mockPerformerRepo.Object);

            this.performerService = new PerformerService(this.factoryMock.Object, this.mapper);
            OperationResult result = performerService.DeletePerformer(id);

            Assert.IsTrue(result.ResultCode == ResultCode.SUCCESS);
        }

        [TestMethod]
        public void DeleteUnexistentPerformerFailsTest()
        {
            var id = 1;

            this.mockPerformerRepo.Setup(m => m.Delete(It.IsAny<int>())).Throws(new RepositoryException());
            this.factoryMock.Setup(m => m.GetRepository(typeof(PerformerEntity))).Returns(this.mockPerformerRepo.Object);

            this.performerService = new PerformerService(this.factoryMock.Object, this.mapper);
            OperationResult result = performerService.DeletePerformer(id);

            Assert.IsTrue(result.ResultCode == ResultCode.FAIL);
        }

        [TestMethod]
        public void UpdatePerformerSuccesfullyTest()
        {
            var updateRequest = new UpdatePerformerRequest
            {
                Name = performer.Name
            };

            this.mockPerformerRepo.Setup(m => m.Update(It.IsAny<PerformerEntity>())).Verifiable();
            this.factoryMock.Setup(m => m.GetRepository(typeof(PerformerEntity))).Returns(this.mockPerformerRepo.Object);

            this.performerService = new PerformerService(this.factoryMock.Object, this.mapper);
            OperationResult expected = performerService.UpdatePerformer(updateRequest);

            Assert.IsTrue(expected.ResultCode == ResultCode.SUCCESS);
        }

        [TestMethod]
        public void GetPerformerByIdTest()
        {
            int id = 1;
            var dbUser = new PerformerEntity
            {
                Id = id,
                Name = performer.Name,
                Artists = performer.Artists,
                Genre = performer.Genre,
                PerformerType = performer.PerformerType,
                StartYear = performer.StartYear
            };

            this.mockPerformerRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(dbUser);
            this.factoryMock.Setup(m => m.GetRepository(typeof(PerformerEntity))).Returns(this.mockPerformerRepo.Object);

            this.performerService = new PerformerService(this.factoryMock.Object, this.mapper);
            Performer concert = performerService.GetPerformer(id);

            Assert.IsNotNull(concert);
            Assert.IsTrue(id == concert.Id);
        }

        [TestMethod]
        public void GetPerformerByNullIdTest()
        {
            int id = 1;

            PerformerEntity dbUser = null;

            this.mockPerformerRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(dbUser);
            this.factoryMock.Setup(m => m.GetRepository(typeof(PerformerEntity))).Returns(this.mockPerformerRepo.Object);

            this.performerService = new PerformerService(this.factoryMock.Object, this.mapper);
            Performer concert = performerService.GetPerformer(id);

            Assert.IsNull(concert);

        }

        [TestMethod]
        public void GetAllPerformersSuccesfullyTest()
        {
            IEnumerable<PerformerEntity> dbAccounts = new List<PerformerEntity>()
            {
                new PerformerEntity
                {
                    Id = 1,
                    Name = performer.Name,
                    Artists = performer.Artists,
                    Genre = performer.Genre,
                    PerformerType = performer.PerformerType,
                    StartYear = performer.StartYear
                },
                new PerformerEntity
                {
                    Id = 2,
                    Name = "The Party Band",
                    Artists = "Geremy Cajtak|Marcelo López",
                    Genre = new GenreEntity(){ Id = 3, GenreName = "Pachanga"},
                    PerformerType = performer.PerformerType,
                    StartYear = performer.StartYear
                },
                new PerformerEntity
                {
                    Id = 3,
                    Name = "Pepito Perez",
                    Genre = performer.Genre,
                    PerformerType = PerformerType.SOLO_ARTIST,
                    StartYear = "1965"
                },
            };

            this.mockPerformerRepo.Setup(r => r.GetAll()).Returns(dbAccounts);
            this.factoryMock.Setup(m => m.GetRepository(typeof(PerformerEntity))).Returns(this.mockPerformerRepo.Object);

            this.performerService = new PerformerService(this.factoryMock.Object, this.mapper);
            IEnumerable<Performer> result = performerService.GetPerformers();

            Assert.IsTrue(result.ToList().Count == 3);
        }
    }
}
