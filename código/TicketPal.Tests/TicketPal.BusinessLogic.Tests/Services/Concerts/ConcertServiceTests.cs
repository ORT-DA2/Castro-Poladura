using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TicketPal.BusinessLogic.Services.Concerts;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Exceptions;
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
                GenreName = "Pop"
            };

            artist = new PerformerEntity()
            {
                Id = 1,
                Name = "Taylor Swift",
                Genre = genre,
                PerformerType = Domain.Constants.PerformerType.SOLO_ARTIST,
                StartYear = "2004"
            };

            concertRequest = new AddConcertRequest()
            {
                Artist = artist,
                Date = DateTime.Now,
                AvailableTickets = 30000,
                EventType = Domain.Constants.EventType.CONCERT,
                TicketPrice = 150,
                CurrencyType = CurrencyType.USD,
                TourName = "Fearless Tour"
            };

            this.concertsMock.Setup(m => m.Exists(It.IsAny<int>())).Returns(false);
            this.concertsMock.Setup(m => m.Add(It.IsAny<ConcertEntity>())).Verifiable();

            concertService = new ConcertService(this.concertsMock.Object, this.testAppSettings, this.mapper);
        }

        [TestMethod]
        public void AddConcertSuccesfullyTest()
        {
            OperationResult result = concertService.AddConcert(concertRequest);

            Assert.IsTrue(result.ResultCode == ResultCode.SUCCESS);
        }

        [TestMethod]
        public void AddConcertTwiceFailsTest()
        {
            concertService.AddConcert(concertRequest);

            this.concertsMock.Setup(m => m.Exists(It.IsAny<int>())).Returns(true);
            this.concertsMock.Setup(m => m.Add(It.IsAny<ConcertEntity>())).Throws(new RepositoryException());
            concertService = new ConcertService(this.concertsMock.Object, this.testAppSettings, this.mapper);

            OperationResult result = concertService.AddConcert(concertRequest);

            Assert.IsTrue(result.ResultCode == ResultCode.FAIL);
        }

        [TestMethod]
        public void DeleteConcertSuccesfullyTest()
        {
            var id = 1;
            var dbUser = new ConcertEntity
            {
                Id = id,
                Artist = concertRequest.Artist,
                Date = concertRequest.Date,
                AvailableTickets = concertRequest.AvailableTickets,
                EventType = concertRequest.EventType,
                TicketPrice = concertRequest.TicketPrice,
                CurrencyType = concertRequest.CurrencyType,
                TourName = concertRequest.TourName
            };

            this.concertsMock.Setup(m => m.Get(It.IsAny<int>())).Returns(dbUser);
            OperationResult result = concertService.DeleteConcert(id);

            Assert.IsTrue(result.ResultCode == ResultCode.SUCCESS);
        }

        [TestMethod]
        public void DeleteUnexistentConcertFailsTest()
        {
            var id = 1;

            this.concertsMock.Setup(m => m.Delete(It.IsAny<int>())).Throws(new RepositoryException());
            this.concertService = new ConcertService(this.concertsMock.Object, this.testAppSettings, this.mapper);
            OperationResult result = concertService.DeleteConcert(id);

            Assert.IsTrue(result.ResultCode == ResultCode.FAIL);
        }

        [TestMethod]
        public void UpdateConcertSuccesfullyTest()
        {
            var tourName = "The Tour";
            var updateRequest = new UpdateConcertRequest
            {
                Artist = concertRequest.Artist,
                Date = concertRequest.Date,
                AvailableTickets = concertRequest.AvailableTickets,
                EventType = concertRequest.EventType,
                TicketPrice = concertRequest.TicketPrice,
                CurrencyType = concertRequest.CurrencyType,
                TourName = tourName
            };

            this.concertsMock.Setup(m => m.Update(It.IsAny<ConcertEntity>())).Verifiable();
            OperationResult expected = concertService.UpdateConcert(updateRequest);

            Assert.IsTrue(expected.ResultCode == ResultCode.SUCCESS);
        }

        [TestMethod]
        public void GetUserByIdTest()
        {
            int id = 1;
            var dbUser = new ConcertEntity
            {
                Id = id,
                Artist = concertRequest.Artist,
                Date = concertRequest.Date,
                AvailableTickets = concertRequest.AvailableTickets,
                EventType = concertRequest.EventType,
                TicketPrice = concertRequest.TicketPrice,
                CurrencyType = concertRequest.CurrencyType,
                TourName = concertRequest.TourName
            };

            this.concertsMock.Setup(r => r.Get(It.IsAny<int>())).Returns(dbUser);
            this.concertService = new ConcertService(this.concertsMock.Object, this.testAppSettings, this.mapper);
            Concert concert = concertService.GetConcert(id);

            Assert.IsNotNull(concert);
            Assert.IsTrue(id == concert.Id);
        }

        [TestMethod]
        public void GetConcertByNullIdTest()
        {
            int id = 1;

            ConcertEntity dbUser = null;

            this.concertsMock.Setup(r => r.Get(It.IsAny<int>())).Returns(dbUser);
            this.concertService = new ConcertService(this.concertsMock.Object, this.testAppSettings, this.mapper);
            Concert concert = concertService.GetConcert(id);

            Assert.IsNull(concert);

        }

        [TestMethod]
        public void GetAllConcertsSuccesfullyTest()
        {
            var artist2 = new PerformerEntity()
            {
                Id = 2,
                Genre = genre,
                Name = "George Michael",
                PerformerType = PerformerType.SOLO_ARTIST,
                StartYear = "1981"
            };

            var artist3 = new PerformerEntity()
            {
                Id = 3,
                Genre = genre,
                Name = "Boy George",
                PerformerType = PerformerType.SOLO_ARTIST,
                StartYear = "1981"
            };

            IEnumerable<ConcertEntity> dbAccounts = new List<ConcertEntity>()
            {
                new ConcertEntity
                {
                    Id = 1,
                    Artist = concertRequest.Artist,
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
                    Artist = artist2,
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
                    Artist = artist3,
                    Date = concertRequest.Date.AddDays(35),
                    AvailableTickets = concertRequest.AvailableTickets,
                    EventType = concertRequest.EventType,
                    TicketPrice = concertRequest.TicketPrice,
                    CurrencyType = concertRequest.CurrencyType,
                    TourName = "Karma Camaleon"
                },
            };
            
            this.concertsMock.Setup(r => r.GetAll()).Returns(dbAccounts);
            this.concertService = new ConcertService(this.concertsMock.Object, this.testAppSettings, this.mapper);
            IEnumerable<Concert> result = concertService.GetConcerts();

            Assert.IsTrue(result.ToList().Count == 3);
        }
    }
}
