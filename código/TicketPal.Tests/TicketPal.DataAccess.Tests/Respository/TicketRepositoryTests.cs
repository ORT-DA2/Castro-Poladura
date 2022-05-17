using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using TicketPal.DataAccess.Repository;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Exceptions;

namespace TicketPal.DataAccess.Tests.Respository
{
    [TestClass]
    public class TicketRepositoryTests : RepositoryBaseConfigTests
    {
        private PerformerEntity performer;
        private ConcertEntity concert;
        private UserEntity user;
        private TicketEntity ticket;
        private string idCode;

        [TestInitialize]
        public void SetUp()
        {
            idCode = "1270f7sdf897adfhlajbvlaaotoaweyoi2";

            performer = new PerformerEntity()
            {
                Id = 1,
                Name = "Berugo Carámbula",
                StartYear = "1963",
                PerformerType = Constants.PERFORMER_TYPE_SOLO_ARTIST,
                Genre = new GenreEntity()
                {
                    Id = 1,
                    GenreName = "Classic"
                },
            };

            concert = new ConcertEntity()
            {
                Id = 1,
                Artist = performer,
                AvailableTickets = 15000,
                CurrencyType = Constants.CURRENCY_URUGUAYAN_PESO,
                Date = DateTime.Now,
                EventType = Constants.EVENT_CONCERT_TYPE,
                TicketPrice = 1800,
                TourName = "Solo de guitarra"
            };

            user = new UserEntity()
            {
                Id = 1,
                Firstname = "Susan",
                Lastname = "Green",
                Email = "sgreen@gmail.com",
                Password = "bfu92i3r7fu138#!%RG983267VWQE!w$^&",
                Role = "SPECTATOR"
            };

            ticket = new TicketEntity()
            {
                Id = 1,
                Event = concert,
                Buyer = user,
                PurchaseDate = DateTime.Now,
                Code = idCode,
                Status = Constants.TICKET_PURCHASED_STATUS
            };

            var eventRepository = new ConcertRepository(dbContext);
            eventRepository.Clear();
            eventRepository.Add(concert);
        }

        [TestMethod]
        public void SaveTicketSuccessfully()
        {
            var code = idCode;
            var repository = new TicketRepository(dbContext);
            repository.Add(ticket);

            Assert.IsTrue(repository.Get(ticket.Id).Event.AvailableTickets == ticket.Event.AvailableTickets);
            Assert.IsTrue(repository.GetAll().ToList().FirstOrDefault().Code == code);
        }

        [TestMethod]
        [ExpectedException(typeof(RepositoryException))]
        public void SaveDuplicateTicketsShouldThrowException()
        {
            var ticket2 = new TicketEntity()
            {
                Id = 1,
                Event = concert,
                Buyer = user,
                PurchaseDate = DateTime.Now,
                Code = idCode,
                Status = Constants.TICKET_PURCHASED_STATUS
            };

            var repository = new TicketRepository(dbContext);
            repository.Add(ticket);
            repository.Add(ticket2);
        }

        [TestMethod]
        [ExpectedException(typeof(RepositoryException))]
        public void SaveTicketWithNoEventsShouldThrowException()
        {
            var ticket2 = new TicketEntity()
            {
                Id = 1,
                Event = concert,
                Buyer = user,
                PurchaseDate = DateTime.Now,
                Code = idCode,
                Status = Constants.TICKET_PURCHASED_STATUS
            };
            var eventRepository = new ConcertRepository(dbContext);
            eventRepository.Clear();

            var repository = new TicketRepository(dbContext);
            repository.Add(ticket);
        }

        [TestMethod]
        [ExpectedException(typeof(RepositoryException))]
        public void SaveTicketForEventWithNoAvailableTicketsShouldThrowException()
        {
            var ticket = new TicketEntity()
            {
                Id = 1,
                Event = new ConcertEntity 
                {
                    Id = 1,
                    Artist = performer,
                    AvailableTickets = 1,
                    CurrencyType = Constants.CURRENCY_URUGUAYAN_PESO,
                    Date = DateTime.Now,
                    EventType = Constants.EVENT_CONCERT_TYPE,
                    TicketPrice = 1800,
                    TourName = "Solo de guitarra"
                },  
                Buyer = user,
                PurchaseDate = DateTime.Now,
                Code = idCode,
                Status = Constants.TICKET_PURCHASED_STATUS
            };

            var eventRepository = new ConcertRepository(dbContext);
            eventRepository.Clear();
            eventRepository.Add(
                new ConcertEntity 
                {
                    Id = 1,
                    Artist = performer,
                    AvailableTickets = 0,
                    CurrencyType = Constants.CURRENCY_URUGUAYAN_PESO,
                    Date = DateTime.Now,
                    EventType = Constants.EVENT_CONCERT_TYPE,
                    TicketPrice = 1800,
                    TourName = "Solo de guitarra"
                }
            );
            var repository = new TicketRepository(dbContext);
            repository.Add(ticket);
        }


        [TestMethod]
        public void ClearRepositoryShouldReturnCountCero()
        {
            var repository = new TicketRepository(dbContext);
            repository.Add(ticket);
            repository.Clear();

            Assert.IsTrue(repository.GetAll().ToList().Count == 0);
        }

        [TestMethod]
        public void ShouldRepositoryBeEmptyWhenEventDeleted()
        {
            var repository = new TicketRepository(dbContext);
            repository.Add(ticket);
            repository.Delete(ticket.Id);

            Assert.IsTrue(repository.IsEmpty());
        }

        [TestMethod]
        public void ExistTicketByIdReturnsTrue()
        {
            var repository = new TicketRepository(dbContext);
            repository.Add(ticket);

            Assert.IsTrue(repository.Exists(ticket.Id));
        }

        [TestMethod]
        public void DeletedTicketShouldNotExist()
        {
            var repository = new TicketRepository(dbContext);
            repository.Add(ticket);
            repository.Delete(ticket.Id);

            Assert.IsFalse(repository.Exists(ticket.Id));
        }

        [TestMethod]
        [ExpectedException(typeof(RepositoryException))]
        public void DeleteNonExistentTicketThrowsException()
        {
            var repository = new TicketRepository(dbContext);
            repository.Delete(ticket.Id);
        }

        [TestMethod]
        public void SearchTicketByCodeTestCorrect()
        {
            var repository = new TicketRepository(dbContext);
            repository.Add(ticket);

            TicketEntity found = repository.Get(g => g.Code.Equals(ticket.Code));

            Assert.IsNotNull(found);
            Assert.AreEqual(ticket.Code, found.Code);
        }

        
        [TestMethod]
        public void ShouldCreatedDateBeTheSameDay()
        {
            string idCode2 = "dfcnwru382w7ghqwrodugnvqpow34r8hg";

            var concert2 = new ConcertEntity()
            {
                Id = 2,
                Artist = performer,
                AvailableTickets = 9800,
                CurrencyType = Constants.CURRENCY_URUGUAYAN_PESO,
                Date = DateTime.Now,
                EventType = Constants.EVENT_CONCERT_TYPE,
                TicketPrice = 1950,
                TourName = "Solo de piano"
            };

            var ticket2 = new TicketEntity()
            {
                Id = 2,
                Event = concert,
                Buyer = user,
                PurchaseDate = DateTime.Now,
                Code = idCode2,
                Status = Constants.TICKET_PURCHASED_STATUS
            };

            var repository = new TicketRepository(dbContext);
            repository.Add(ticket);
            repository.Add(ticket2);
            Assert.IsTrue(
                ticket.CreatedAt.Day.Equals(ticket2.CreatedAt.Day)
                && ticket.CreatedAt.Year.Equals(ticket2.CreatedAt.Year)
                && ticket.CreatedAt.Month.Equals(ticket2.CreatedAt.Month));
        }

        [TestMethod]
        public void UpdateEntityValuesSuccessfully()
        {
            var repository = new TicketRepository(dbContext);
            repository.Add(ticket);

            var purchaseDate = new DateTime(2022, 04, 20);

            ticket.PurchaseDate = purchaseDate;

            repository.Update(ticket);

            var updatedEvent = repository.Get(ticket.Id);

            Assert.IsTrue(updatedEvent.PurchaseDate.Equals(purchaseDate));
        }

        [TestMethod]
        public void UpdateEntityValuesNullThenShouldMantainPreviousValues()
        {
            var repository = new TicketRepository(dbContext);
            repository.Add(ticket);

            repository.Update(new TicketEntity { Id = ticket.Id, Buyer = user, Event = concert, PurchaseDate = DateTime.Now, Status = Constants.TICKET_PURCHASED_STATUS, Code = null });

            var updatedEvent = repository.Get(ticket.Id);


            Assert.IsTrue(updatedEvent.Code.Equals(ticket.Code));
        }
    }
}
