using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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
        private PerformerEntity performer;
        private AddPerformerRequest performerRequest;
        private PerformerService performerService;

        [TestInitialize]
        public void Initialize()
        {
            genre = new GenreEntity()
            {
                Id = 1,
                Name = "Pop Rock"
            };

            performer = new PerformerEntity()
            {
                Id = 1,
                UserInfo = new UserEntity { Firstname = "Coldplay" },
                PerformerType = Constants.PERFORMER_TYPE_BAND,
                StartYear = "1996",
                Genre = genre,
                Members = new List<PerformerEntity>()
            };

            performerRequest = new AddPerformerRequest()
            {
                MembersIds = new List<int>(),
                Genre = genre.Id,
                UserId = 1,
                PerformerType = performer.PerformerType,
                StartYear = performer.StartYear
            };

            this.mockPerformerRepo.Setup(m => m.Exists(It.IsAny<int>())).Returns(false);
            this.mockPerformerRepo.Setup(m => m.Add(It.IsAny<PerformerEntity>())).Verifiable();
            this.mockUserRepo.Setup(u => u.Get(It.IsAny<int>())).Returns(Task.FromResult(new UserEntity { Role = Constants.ROLE_ARTIST }));
            this.mockConcertRepo.Setup(c => c.GetAll(It.IsAny<Expression<Func<ConcertEntity, bool>>>()))
                .Returns(Task.FromResult(new List<ConcertEntity>()));
            this.mockGenreRepo.Setup(m => m.Get(It.IsAny<int>())).Returns(Task.FromResult(genre));

            this.factoryMock.Setup(m => m.GetRepository(typeof(PerformerEntity))).Returns(this.mockPerformerRepo.Object);
            this.factoryMock.Setup(m => m.GetRepository(typeof(GenreEntity))).Returns(this.mockGenreRepo.Object);
            this.factoryMock.Setup(m => m.GetRepository(typeof(UserEntity))).Returns(this.mockUserRepo.Object);
            this.factoryMock.Setup(m => m.GetRepository(typeof(ConcertEntity))).Returns(this.mockConcertRepo.Object);
            this.performerService = new PerformerService(this.factoryMock.Object, this.mapper);
        }

        [TestMethod]
        public async Task AddPerformerSuccesfullyTest()
        {
            OperationResult result = await performerService.AddPerformer(performerRequest);

            Assert.IsTrue(result.ResultCode == Constants.CODE_SUCCESS);
        }

        [TestMethod]
        public async Task AddPerformerTwiceFailsTest()
        {
            await performerService.AddPerformer(performerRequest);

            this.mockPerformerRepo.Setup(m => m.Exists(It.IsAny<int>())).Returns(true);
            this.mockPerformerRepo.Setup(m => m.Add(It.IsAny<PerformerEntity>())).Throws(new RepositoryException());
            this.factoryMock.Setup(m => m.GetRepository(typeof(PerformerEntity))).Returns(this.mockPerformerRepo.Object);

            this.performerService = new PerformerService(this.factoryMock.Object, this.mapper);

            OperationResult result = await performerService.AddPerformer(performerRequest);

            Assert.IsTrue(result.ResultCode == Constants.CODE_FAIL);
        }

        [TestMethod]
        public async Task AddPerformerWithNoExistentGenreTest()
        {
            this.mockGenreRepo.Setup(m => m.Get(It.IsAny<int>()));

            this.factoryMock.Setup(m => m.GetRepository(typeof(PerformerEntity))).Returns(this.mockPerformerRepo.Object);
            this.factoryMock.Setup(m => m.GetRepository(typeof(GenreEntity))).Returns(this.mockGenreRepo.Object);

            this.performerService = new PerformerService(this.factoryMock.Object, this.mapper);

            OperationResult result = await performerService.AddPerformer(performerRequest);

            Assert.IsTrue(result.ResultCode == Constants.CODE_FAIL);
        }

        [TestMethod]
        public async Task DeletePerformerSuccesfullyTest()
        {
            var id = 1;
            var dbUser = new PerformerEntity
            {
                Id = id,
                UserInfo = performer.UserInfo,
                Members = performer.Members,
                Genre = performer.Genre,
                PerformerType = performer.PerformerType,
                StartYear = performer.StartYear
            };

            this.mockPerformerRepo.Setup(m => m.Get(It.IsAny<int>())).Returns(Task.FromResult(dbUser));
            this.factoryMock.Setup(m => m.GetRepository(typeof(PerformerEntity))).Returns(this.mockPerformerRepo.Object);

            this.performerService = new PerformerService(this.factoryMock.Object, this.mapper);
            OperationResult result = await performerService.DeletePerformer(id);

            Assert.IsTrue(result.ResultCode == Constants.CODE_SUCCESS);
        }

        [TestMethod]
        public async Task DeleteUnexistentPerformerFailsTest()
        {
            var id = 1;

            this.mockPerformerRepo.Setup(m => m.Delete(It.IsAny<int>())).Throws(new RepositoryException());
            this.factoryMock.Setup(m => m.GetRepository(typeof(PerformerEntity))).Returns(this.mockPerformerRepo.Object);

            this.performerService = new PerformerService(this.factoryMock.Object, this.mapper);
            OperationResult result = await performerService.DeletePerformer(id);

            Assert.IsTrue(result.ResultCode == Constants.CODE_FAIL);
        }

        [TestMethod]
        public async Task UpdatePerformerSuccesfullyTest()
        {
            var updateRequest = new UpdatePerformerRequest
            {
                UserId = 1
            };

            this.mockPerformerRepo.Setup(m => m.Update(It.IsAny<PerformerEntity>())).Verifiable();
            this.mockUserRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(
                Task.FromResult(
                    new UserEntity
                    {
                        Performer = new PerformerEntity { Members = new List<PerformerEntity>() }
                    })
                );
            this.mockPerformerRepo.Setup(m => m.GetAll(It.IsAny<Expression<Func<PerformerEntity, bool>>>()))
            .Returns(Task.FromResult(new List<PerformerEntity>()));
            this.factoryMock.Setup(m => m.GetRepository(typeof(UserEntity))).Returns(this.mockUserRepo.Object);
            this.factoryMock.Setup(m => m.GetRepository(typeof(PerformerEntity))).Returns(this.mockPerformerRepo.Object);

            this.performerService = new PerformerService(this.factoryMock.Object, this.mapper);
            OperationResult expected = await performerService.UpdatePerformer(updateRequest);

            Assert.IsTrue(expected.ResultCode == Constants.CODE_SUCCESS);
        }

        [TestMethod]
        public async Task GetPerformerByIdTest()
        {
            int id = 1;
            var dbUser = new PerformerEntity
            {
                Id = id,
                UserInfo = performer.UserInfo,
                Members = performer.Members,
                Genre = performer.Genre,
                PerformerType = performer.PerformerType,
                StartYear = performer.StartYear
            };

            this.mockPerformerRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(Task.FromResult(dbUser));
            this.factoryMock.Setup(m => m.GetRepository(typeof(PerformerEntity))).Returns(this.mockPerformerRepo.Object);

            this.performerService = new PerformerService(this.factoryMock.Object, this.mapper);
            Performer res = await performerService.GetPerformer(id);

            Assert.IsNotNull(res);
            Assert.IsTrue(id == res.Id);
        }

        [TestMethod]
        public async Task GetPerformerByNullIdTest()
        {
            int id = 1;

            PerformerEntity dbUser = null;

            this.mockPerformerRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(Task.FromResult(dbUser));
            this.factoryMock.Setup(m => m.GetRepository(typeof(PerformerEntity))).Returns(this.mockPerformerRepo.Object);

            this.performerService = new PerformerService(this.factoryMock.Object, this.mapper);
            Performer concert = await performerService.GetPerformer(id);

            Assert.IsNull(concert);

        }

        [TestMethod]
        public async Task GetAllPerformersSuccesfullyTest()
        {
            IEnumerable<PerformerEntity> dbAccounts = new List<PerformerEntity>()
            {
                new PerformerEntity
                {
                    Id = 1,
                    UserInfo = performer.UserInfo,
                    Members = performer.Members,
                    Genre = performer.Genre,
                    PerformerType = performer.PerformerType,
                    StartYear = performer.StartYear
                },
                new PerformerEntity
                {
                    Id = 2,
                    UserInfo = new UserEntity {Firstname = "The Party Band"},
                    Members = new List<PerformerEntity>(),
                    Genre = new GenreEntity(){ Id = 3, Name = "Pachanga"},
                    PerformerType = performer.PerformerType,
                    StartYear = performer.StartYear
                },
                new PerformerEntity
                {
                    Id = 3,
                    UserInfo = new UserEntity { Firstname = "Pepito Perez" },
                    Genre = performer.Genre,
                    PerformerType = Constants.PERFORMER_TYPE_SOLO_ARTIST,
                    StartYear = "1965"
                },
            };

            this.mockPerformerRepo.Setup(r => r.GetAll()).Returns(Task.FromResult(dbAccounts.ToList()));
            this.factoryMock.Setup(m => m.GetRepository(typeof(PerformerEntity))).Returns(this.mockPerformerRepo.Object);

            this.performerService = new PerformerService(this.factoryMock.Object, this.mapper);
            IEnumerable<Performer> result = await performerService.GetPerformers();

            Assert.IsTrue(result.ToList().Count == 3);
        }
    }
}
