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

        [TestInitialize]
        public void SetUp()
        {
            performer = new PerformerEntity()
            {
                Id = 1,
                Name = "Berugo Carámbula",
                StartYear = "1963",
                PerformerType = PerformerType.SOLO_ARTIST,
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
                CurrencyType = CurrencyType.UYU,
                Date = DateTime.Now,
                EventType = EventType.CONCERT,
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
                ShowName = concert.TourName,
                Status = TicketStatus.PURCHASED
            };
        }

        [TestMethod]
        public void SaveTicketSuccessfully()
        {
            var tourName = concert.TourName;
            var repository = new ConcertRepository(dbContext);
            repository.Add(concert);

            Assert.IsTrue(repository.GetAll().ToList().FirstOrDefault().TourName == tourName);
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
                ShowName = concert.TourName,
                Status = TicketStatus.PURCHASED
            };

            var repository = new TicketRepository(dbContext);
            repository.Add(ticket);
            repository.Add(ticket2);
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
        public void SearchTicketByShowNameTestCorrect()
        {
            var repository = new TicketRepository(dbContext);
            repository.Add(ticket);

            TicketEntity found = repository.Get(g => g.ShowName.Equals(ticket.ShowName));

            Assert.IsNotNull(found);
            Assert.AreEqual(ticket.ShowName, found.ShowName);
        }

        
        [TestMethod]
        public void ShouldCreatedDateBeTheSameDay()
        {
            var concert2 = new ConcertEntity()
            {
                Id = 2,
                Artist = performer,
                AvailableTickets = 9800,
                CurrencyType = CurrencyType.UYU,
                Date = DateTime.Now,
                EventType = EventType.CONCERT,
                TicketPrice = 1950,
                TourName = "Solo de piano"
            };

            var ticket2 = new TicketEntity()
            {
                Id = 2,
                Event = concert2,
                Buyer = user,
                PurchaseDate = DateTime.Now,
                ShowName = concert2.TourName,
                Status = TicketStatus.PURCHASED
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

            var newName = "Bien de bien";

            ticket.ShowName = newName;

            repository.Update(ticket);

            var updatedEvent = repository.Get(ticket.Id);

            Assert.IsTrue(updatedEvent.ShowName.Equals(newName));
        }

        [TestMethod]
        public void UpdateEntityValuesNullThenShouldMantainPreviousValues()
        {
            var repository = new TicketRepository(dbContext);
            repository.Add(ticket);

            repository.Update(new TicketEntity { Id = ticket.Id, Buyer = user, Event = concert, PurchaseDate = DateTime.Now, Status = TicketStatus.PURCHASED, ShowName = null });

            var updatedEvent = repository.Get(ticket.Id);


            Assert.IsTrue(updatedEvent.ShowName.Equals(ticket.ShowName));
        }
    }
}
