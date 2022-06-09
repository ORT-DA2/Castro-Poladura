using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TicketPal.BusinessLogic.Services.Concerts;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Exceptions;
using TicketPal.Domain.Models.Param;
using TicketPal.Domain.Models.Request;
using TicketPal.Domain.Models.Response;

namespace TicketPal.BusinessLogic.Tests.Services.Concerts
{
    [TestClass]
    public class ConcertServiceTests : BaseServiceTests
    {
        private GenreEntity genre;
        private PerformerEntity artist;
        private AddConcertRequest concertRequest;
        private ConcertService concertService;

        [TestInitialize]
        public void Initialize()
        {

            genre = new GenreEntity()
            {
                Id = 1,
                Name = "Pop"
            };

            artist = new PerformerEntity()
            {
                Id = 1,
                UserInfo = new UserEntity { Firstname = "Taylor Swift" },
                Genre = genre,
                PerformerType = Constants.PERFORMER_TYPE_SOLO_ARTIST,
                StartYear = "2004"
            };

            concertRequest = new AddConcertRequest()
            {
                ArtistsIds = new List<int> { 1 },
                Date = DateTime.Now,
                AvailableTickets = 30000,
                EventType = Constants.EVENT_CONCERT_TYPE,
                TicketPrice = 150,
                CurrencyType = Constants.CURRENCY_US_DOLLARS,
                TourName = "Fearless Tour",
                Location = "Some Location",
                Address = "Some address",
                Country = "Country"
            };

            this.mockConcertRepo.Setup(m => m.Exists(It.IsAny<int>())).Returns(false);
            this.mockConcertRepo.Setup(m => m.Add(It.IsAny<ConcertEntity>())).Verifiable();

            this.mockPerformerRepo.Setup(m => m.Get(It.IsAny<int>())).Returns(Task.FromResult(artist));
        }

        [TestMethod]
        public async Task AddConcertSuccesfullyTest()
        {
            this.mockConcertRepo.Setup(m => m.Get(It.IsAny<Expression<Func<ConcertEntity, bool>>>()))
                .Returns(Task.FromResult(It.IsAny<ConcertEntity>()));
            this.factoryMock.Setup(m => m.GetRepository(typeof(PerformerEntity))).Returns(this.mockPerformerRepo.Object);
            this.factoryMock.Setup(f => f.GetRepository(typeof(ConcertEntity))).Returns(this.mockConcertRepo.Object);
            this.concertService = new ConcertService(this.factoryMock.Object, this.mapper);
            OperationResult result = await concertService.AddConcert(concertRequest);

            Assert.IsTrue(result.ResultCode == Constants.CODE_SUCCESS);
        }

        [TestMethod]
        public async Task AddConcertTwiceFailsTest()
        {

            this.mockConcertRepo.Setup(m => m.Exists(It.IsAny<int>())).Returns(true);
            this.mockConcertRepo.Setup(m => m.Add(It.IsAny<ConcertEntity>())).Throws(new RepositoryException());
            this.factoryMock.Setup(m => m.GetRepository(typeof(ConcertEntity))).Returns(this.mockConcertRepo.Object);
            this.factoryMock.Setup(m => m.GetRepository(typeof(PerformerEntity))).Returns(this.mockPerformerRepo.Object);
            this.concertService = new ConcertService(this.factoryMock.Object, this.mapper);

            OperationResult result = await concertService.AddConcert(concertRequest);

            Assert.IsTrue(result.ResultCode == Constants.CODE_FAIL);
        }

        [TestMethod]
        public async Task AddConcertWithExistentArtistTest()
        {
            this.mockConcertRepo.Setup(m => m.Get(It.IsAny<Expression<Func<ConcertEntity, bool>>>())).Returns(Task.FromResult(new ConcertEntity()));

            this.factoryMock.Setup(m => m.GetRepository(typeof(ConcertEntity))).Returns(this.mockConcertRepo.Object);
            this.factoryMock.Setup(m => m.GetRepository(typeof(PerformerEntity))).Returns(this.mockPerformerRepo.Object);

            this.concertService = new ConcertService(this.factoryMock.Object, this.mapper);

            OperationResult result = await concertService.AddConcert(concertRequest);

            Assert.IsTrue(result.ResultCode == Constants.CODE_FAIL);
        }

        [TestMethod]
        public async Task DeleteConcertSuccesfullyTest()
        {
            var id = 1;
            var dbUser = new ConcertEntity
            {
                Id = id,
                Artists = new List<PerformerEntity>(),
                Date = concertRequest.Date,
                AvailableTickets = concertRequest.AvailableTickets,
                EventType = concertRequest.EventType,
                TicketPrice = concertRequest.TicketPrice,
                CurrencyType = concertRequest.CurrencyType,
                TourName = concertRequest.TourName
            };

            this.mockConcertRepo.Setup(m => m.Get(It.IsAny<int>())).Returns(Task.FromResult(dbUser));
            this.factoryMock.Setup(m => m.GetRepository(typeof(ConcertEntity))).Returns(this.mockConcertRepo.Object);
            this.concertService = new ConcertService(this.factoryMock.Object, this.mapper);
            OperationResult result = await concertService.DeleteConcert(id);

            Assert.IsTrue(result.ResultCode == Constants.CODE_SUCCESS);
        }

        [TestMethod]
        public async Task DeleteUnexistentConcertFailsTest()
        {
            var id = 1;

            this.mockConcertRepo.Setup(m => m.Delete(It.IsAny<int>())).Throws(new RepositoryException());
            this.factoryMock.Setup(m => m.GetRepository(typeof(ConcertEntity))).Returns(this.mockConcertRepo.Object);
            this.concertService = new ConcertService(this.factoryMock.Object, this.mapper);
            OperationResult result = await concertService.DeleteConcert(id);

            Assert.IsTrue(result.ResultCode == Constants.CODE_FAIL);
        }

        [TestMethod]
        public async Task UpdateConcertSuccesfullyTest()
        {
            var tourName = "The Tour";
            var updateRequest = new UpdateConcertRequest
            {
                Date = concertRequest.Date,
                EventType = concertRequest.EventType,
                TicketPrice = concertRequest.TicketPrice,
                CurrencyType = concertRequest.CurrencyType,
                TourName = tourName
            };

            this.mockConcertRepo.Setup(m => m.Update(It.IsAny<ConcertEntity>())).Verifiable();
            this.mockConcertRepo.Setup(c => c.Get(It.IsAny<int>())).Returns(
                Task.FromResult(new ConcertEntity { Artists = new List<PerformerEntity>() })
            );
            this.mockPerformerRepo.Setup(m => m.GetAll(It.IsAny<Expression<Func<PerformerEntity, bool>>>()))
            .Returns(Task.FromResult(new List<PerformerEntity>()));
            this.factoryMock.Setup(f => f.GetRepository(typeof(ConcertEntity))).Returns(this.mockConcertRepo.Object);
            this.factoryMock.Setup(f => f.GetRepository(typeof(PerformerEntity))).Returns(this.mockPerformerRepo.Object);
            this.concertService = new ConcertService(this.factoryMock.Object, this.mapper);
            OperationResult expected = await concertService.UpdateConcert(updateRequest);

            Assert.IsTrue(expected.ResultCode == Constants.CODE_SUCCESS);
        }

        [TestMethod]
        public async Task GetUserByIdTest()
        {
            int id = 1;
            var dbUser = new ConcertEntity
            {
                Id = id,
                Artists = new List<PerformerEntity>(),
                Date = concertRequest.Date,
                AvailableTickets = concertRequest.AvailableTickets,
                EventType = concertRequest.EventType,
                TicketPrice = concertRequest.TicketPrice,
                CurrencyType = concertRequest.CurrencyType,
                TourName = concertRequest.TourName
            };

            this.mockConcertRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(Task.FromResult(dbUser));
            this.factoryMock.Setup(m => m.GetRepository(typeof(ConcertEntity))).Returns(this.mockConcertRepo.Object);
            this.concertService = new ConcertService(this.factoryMock.Object, this.mapper);
            var concert = await concertService.GetConcert(id);

            Assert.IsNotNull(concert);
            Assert.IsTrue(id == concert.Id);
        }

        [TestMethod]
        public async Task GetConcertByNullIdTest()
        {
            int id = 1;

            ConcertEntity dbUser = null;

            this.mockConcertRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(Task.FromResult(dbUser));
            this.factoryMock.Setup(m => m.GetRepository(typeof(ConcertEntity)))
                .Returns(this.mockConcertRepo.Object);

            this.concertService = new ConcertService(this.factoryMock.Object, this.mapper);
            var concert = await concertService.GetConcert(id);

            Assert.IsNull(concert);

        }

        [TestMethod]
        public async Task GetAllConcertsTest()
        {
            var artist2 = new PerformerEntity()
            {
                Id = 2,
                Genre = genre,
                UserInfo = new UserEntity { Firstname = "George Michael" },
                PerformerType = Constants.PERFORMER_TYPE_SOLO_ARTIST,
                StartYear = "1981"
            };

            var artist3 = new PerformerEntity()
            {
                Id = 3,
                Genre = genre,
                UserInfo = new UserEntity { Firstname = "Boy George" },
                PerformerType = Constants.PERFORMER_TYPE_SOLO_ARTIST,
                StartYear = "1981"
            };

            IEnumerable<ConcertEntity> dbAccounts = new List<ConcertEntity>()
            {
                new ConcertEntity
                {
                    Id = 1,
                    Artists = new List<PerformerEntity>(),
                    Date = concertRequest.Date,
                    AvailableTickets = concertRequest.AvailableTickets,
                    EventType = concertRequest.EventType,
                    TicketPrice = concertRequest.TicketPrice,
                    CurrencyType = concertRequest.CurrencyType,
                    TourName = concertRequest.TourName
                },
                new ConcertEntity
                {
                    Id = 2,
                    Artists = new List<PerformerEntity>(),
                    Date = concertRequest.Date.AddDays(25),
                    AvailableTickets = concertRequest.AvailableTickets,
                    EventType = concertRequest.EventType,
                    TicketPrice = concertRequest.TicketPrice,
                    CurrencyType = concertRequest.CurrencyType,
                    TourName = "Faith"
                },
                new ConcertEntity
                {
                    Id = 3,
                    Artists = new List<PerformerEntity>(),
                    Date = concertRequest.Date.AddDays(35),
                    AvailableTickets = concertRequest.AvailableTickets,
                    EventType = concertRequest.EventType,
                    TicketPrice = concertRequest.TicketPrice,
                    CurrencyType = concertRequest.CurrencyType,
                    TourName = "Karma Camaleon"
                },
            };

            this.mockConcertRepo.Setup(r => r.GetAll(It.IsAny<Expression<Func<ConcertEntity, bool>>>())).Returns(Task.FromResult(dbAccounts.ToList()));
            this.factoryMock.Setup(m => m.GetRepository(typeof(ConcertEntity))).Returns(this.mockConcertRepo.Object);
            this.concertService = new ConcertService(this.factoryMock.Object, this.mapper);
            IEnumerable<Concert> result = await concertService.GetConcerts(
                new ConcertSearchParam
                {
                    Type = Constants.EVENT_CONCERT_TYPE,
                    Newest = true,
                    StartDate = DateTime.Now.AddDays(30).ToString("dd/M/yyyy hh:mm"),
                    EndDate = null,
                    ArtistName = null,
                    TourName = null
                }
            );

            result = await concertService.GetConcerts(
                new ConcertSearchParam
                {
                    Type = Constants.EVENT_CONCERT_TYPE,
                    Newest = true,
                    StartDate = null,
                    EndDate = DateTime.Now.AddDays(30).ToString("dd/M/yyyy hh:mm"),
                    ArtistName = null,
                    TourName = null
                }
            );

            result = await concertService.GetConcerts(
                new ConcertSearchParam
                {
                    Type = Constants.EVENT_CONCERT_TYPE,
                    Newest = true,
                    StartDate = DateTime.Now.AddDays(30).ToString("dd/M/yyyy hh:mm"),
                    EndDate = DateTime.Now.AddDays(30).ToString("dd/M/yyyy hh:mm"),
                    ArtistName = null,
                    TourName = null
                }
            );

            result = await concertService.GetConcerts(
                new ConcertSearchParam
                {
                    Type = Constants.EVENT_CONCERT_TYPE,
                    Newest = true,
                    StartDate = DateTime.Now.AddDays(30).ToString("dd/M/yyyy hh:mm"),
                    EndDate = DateTime.Now.AddDays(30).ToString("dd/M/yyyy hh:mm"),
                    ArtistName = "someArtist",
                    TourName = null
                }
            );

            result = await concertService.GetConcerts(
                new ConcertSearchParam
                {
                    Type = Constants.EVENT_CONCERT_TYPE,
                    Newest = true,
                    StartDate = DateTime.Now.AddDays(30).ToString("dd/M/yyyy hh:mm"),
                    EndDate = DateTime.Now.AddDays(30).ToString("dd/M/yyyy hh:mm"),
                    ArtistName = "someArtist",
                    TourName = "someTourName"
                }
            );

            result = await concertService.GetConcerts(
                new ConcertSearchParam
                {
                    Type = Constants.EVENT_CONCERT_TYPE,
                    Newest = true,
                    StartDate = DateTime.Now.AddDays(30).ToString("dd/M/yyyy hh:mm"),
                    EndDate = DateTime.Now.AddDays(30).ToString("dd/M/yyyy hh:mm"),
                    ArtistName = null,
                    TourName = "someTourName"
                }
            );
            this.mockConcertRepo.Verify(r => r.GetAll(It.IsAny<Expression<Func<ConcertEntity, bool>>>()), () => Times.AtLeastOnce());
        }
    }
}
