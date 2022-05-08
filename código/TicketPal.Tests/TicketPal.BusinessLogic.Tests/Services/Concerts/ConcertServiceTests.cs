﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using TicketPal.BusinessLogic.Services.Concerts;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Exceptions;
using TicketPal.Domain.Models.Request;
using TicketPal.Domain.Models.Response;
using TicketPal.Interfaces.Factory;

namespace TicketPal.BusinessLogic.Tests.Services.Concerts
{
    [TestClass]
    public class ConcertServiceTests : BaseServiceTests
    {
        private GenreEntity genre;
        private PerformerEntity artist;
        private AddConcertRequest concertRequest;
        private ConcertService concertService;
        private IServiceFactory serviceFactory;


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
                Artist = 1,
                Date = DateTime.Now,
                AvailableTickets = 30000,
                EventType = Domain.Constants.EventType.CONCERT,
                TicketPrice = 150,
                CurrencyType = CurrencyType.USD,
                TourName = "Fearless Tour"
            };

            this.mockConcertRepo.Setup(m => m.Exists(It.IsAny<int>())).Returns(false);
            this.mockConcertRepo.Setup(m => m.Add(It.IsAny<ConcertEntity>())).Verifiable();

            this.mockPerformerRepo.Setup(m => m.Get(It.IsAny<int>())).Returns(artist);

            this.factoryMock.Setup(m => m.GetRepository(typeof(ConcertEntity))).Returns(this.mockConcertRepo.Object);
            this.factoryMock.Setup(m => m.GetRepository(typeof(PerformerEntity))).Returns(this.mockPerformerRepo.Object);

            this.concertService = new ConcertService(this.factoryMock.Object, this.mapper);
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

            this.mockConcertRepo.Setup(m => m.Exists(It.IsAny<int>())).Returns(true);
            this.mockConcertRepo.Setup(m => m.Add(It.IsAny<ConcertEntity>())).Throws(new RepositoryException());
            this.factoryMock.Setup(m => m.GetRepository(typeof(ConcertEntity))).Returns(this.mockConcertRepo.Object);
            this.concertService = new ConcertService(this.factoryMock.Object, this.mapper);

            OperationResult result = concertService.AddConcert(concertRequest);

            Assert.IsTrue(result.ResultCode == ResultCode.FAIL);
        }

        [TestMethod]
        public void AddConcertWithNoExistentArtistTest()
        {
            this.mockPerformerRepo.Setup(m => m.Get(It.IsAny<int>()));

            this.factoryMock.Setup(m => m.GetRepository(typeof(ConcertEntity))).Returns(this.mockConcertRepo.Object);
            this.factoryMock.Setup(m => m.GetRepository(typeof(PerformerEntity))).Returns(this.mockPerformerRepo.Object);

            this.concertService = new ConcertService(this.factoryMock.Object, this.mapper);

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
                Artist = artist,
                Date = concertRequest.Date,
                AvailableTickets = concertRequest.AvailableTickets,
                EventType = concertRequest.EventType,
                TicketPrice = concertRequest.TicketPrice,
                CurrencyType = concertRequest.CurrencyType,
                TourName = concertRequest.TourName
            };

            this.mockConcertRepo.Setup(m => m.Get(It.IsAny<int>())).Returns(dbUser);
            this.factoryMock.Setup(m => m.GetRepository(typeof(ConcertEntity))).Returns(this.mockConcertRepo.Object);
            this.concertService = new ConcertService(this.factoryMock.Object, this.mapper);
            OperationResult result = concertService.DeleteConcert(id);

            Assert.IsTrue(result.ResultCode == ResultCode.SUCCESS);
        }

        [TestMethod]
        public void DeleteUnexistentConcertFailsTest()
        {
            var id = 1;

            this.mockConcertRepo.Setup(m => m.Delete(It.IsAny<int>())).Throws(new RepositoryException());
            this.factoryMock.Setup(m => m.GetRepository(typeof(ConcertEntity))).Returns(this.mockConcertRepo.Object);
            this.concertService = new ConcertService(this.factoryMock.Object, this.mapper);
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

            this.mockConcertRepo.Setup(m => m.Update(It.IsAny<ConcertEntity>())).Verifiable();
            this.factoryMock.Setup(m => m.GetRepository(typeof(ConcertEntity))).Returns(this.mockConcertRepo.Object);
            this.concertService = new ConcertService(this.factoryMock.Object, this.mapper);
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
                Artist = artist,
                Date = concertRequest.Date,
                AvailableTickets = concertRequest.AvailableTickets,
                EventType = concertRequest.EventType,
                TicketPrice = concertRequest.TicketPrice,
                CurrencyType = concertRequest.CurrencyType,
                TourName = concertRequest.TourName
            };

            this.mockConcertRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(dbUser);
            this.factoryMock.Setup(m => m.GetRepository(typeof(ConcertEntity))).Returns(this.mockConcertRepo.Object);
            this.concertService = new ConcertService(this.factoryMock.Object, this.mapper);
            Concert concert = concertService.GetConcert(id);

            Assert.IsNotNull(concert);
            Assert.IsTrue(id == concert.Id);
        }

        [TestMethod]
        public void GetConcertByNullIdTest()
        {
            int id = 1;

            ConcertEntity dbUser = null;

            this.mockConcertRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(dbUser);
            this.factoryMock.Setup(m => m.GetRepository(typeof(ConcertEntity)))
                .Returns(this.mockConcertRepo.Object);

            this.concertService = new ConcertService(this.factoryMock.Object, this.mapper);
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
                    Artist = artist,
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
            
            this.mockConcertRepo.Setup(r => r.GetAll()).Returns(dbAccounts);
            this.factoryMock.Setup(m => m.GetRepository(typeof(ConcertEntity))).Returns(this.mockConcertRepo.Object);
            this.concertService = new ConcertService(this.factoryMock.Object, this.mapper);
            IEnumerable<Concert> result = concertService.GetConcerts();

            Assert.IsTrue(result.ToList().Count == 3);
        }
    }
}
