using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using TicketPal.DataAccess.Repository;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Exceptions;

namespace TicketPal.DataAccess.Tests.Respository
{
    [TestClass]
    public class ConcertRepositoryTests : RepositoryBaseConfigTests
    {
        private IEnumerable<PerformerEntity> artists;
        private ConcertEntity concert;
        
        [TestInitialize]
        public void Init() {
            var genre = new GenreEntity {GenreName="Pop"};

            artists = new List<PerformerEntity> {
                new PerformerEntity {
                    UserInfo = new UserEntity { Firstname = "name1" },
                    PerformerType = Constants.PERFORMER_TYPE_SOLO_ARTIST,
                    StartYear = "1987",
                    Genre = genre,
                    Concerts = new List<ConcertEntity>()
                },
                new PerformerEntity {
                    UserInfo = new UserEntity { Firstname = "name2"},
                    PerformerType = Constants.PERFORMER_TYPE_SOLO_ARTIST,
                    StartYear = "2000",
                    Genre = genre,
                    Concerts = new List<ConcertEntity>()
                }
            };

            concert = new ConcertEntity()
            {
                Artists = artists.ToList(),
                AvailableTickets = 500,
                CurrencyType = Constants.CURRENCY_US_DOLLARS,
                Date = DateTime.Now,
                EventType = Constants.EVENT_CONCERT_TYPE,
                TicketPrice = 100,
                TourName = "someTour",
                Location = "some location",
                Address = "some address",
                Country = "some country"
            };

            var repository = new ConcertRepository(dbContext);
            repository.Add(concert);
        }
        [TestMethod]
        public void CheckSavedEvent()
        {
            var repository = new ConcertRepository(dbContext);
            Assert.IsTrue(repository.GetAll().ToList().FirstOrDefault().TourName == "someTour");
        }

        [TestMethod]
        [ExpectedException(typeof(RepositoryException))]
        public void SaveDuplicateEventsShouldThrowException()
        {
            var repository = new ConcertRepository(dbContext);
            repository.Add(concert);
        }


        [TestMethod]
        public void ClearRepositoryShouldReturnCountCero()
        {
            var repository = new ConcertRepository(dbContext);

            Assert.IsTrue(repository.GetAll().ToList().Count == 1);
            repository.Clear();
            Assert.IsTrue(repository.GetAll().ToList().Count == 0);
        }

        [TestMethod]
        public void ShouldRepositoryBeEmptyWhenEventDeleted()
        {

            var repository = new ConcertRepository(dbContext);
            repository.Delete(concert.Id);

            Assert.IsTrue(repository.IsEmpty());
        }

        [TestMethod]
        public void ExistEventByIdReturnsTrue()
        {
            var repository = new ConcertRepository(dbContext);
            Assert.IsTrue(repository.Exists(concert.Id));
        }

        [TestMethod]
        public void DeletedEventShouldNotExist()
        {
            var repository = new ConcertRepository(dbContext);
            repository.Delete(concert.Id);

            Assert.IsFalse(repository.Exists(concert.Id));
        }

        [TestMethod]
        [ExpectedException(typeof(RepositoryException))]
        public void DeleteNonExistentEventThrowsException()
        {
            var repository = new ConcertRepository(dbContext);
            repository.Clear();
            repository.Delete(concert.Id);
        }

        [TestMethod]
        public void SearchEventByNameTestCorrect()
        {
            var repository = new ConcertRepository(dbContext);
            ConcertEntity found = repository.Get(g => g.TourName.Equals(concert.TourName));

            Assert.IsNotNull(found);
            Assert.AreEqual(concert.TourName, found.TourName);
        }

        [TestMethod]
        public void ShouldCreatedDateBeTheSameDay()
        {
            var concert1 = new ConcertEntity()
            {
                Id = 2,
                Artists = artists.ToList(),
                AvailableTickets = 5624,
                CurrencyType = Constants.CURRENCY_URUGUAYAN_PESO,
                Date = DateTime.Now,
                EventType = Constants.EVENT_CONCERT_TYPE,
                TicketPrice = 2000,
                TourName = "Buitres Después de la Una"
            };

            var repository = new ConcertRepository(dbContext);
            repository.Add(concert1);
            Assert.IsTrue(
                concert1.CreatedAt.Day.Equals(concert.CreatedAt.Day)
                && concert1.CreatedAt.Year.Equals(concert.CreatedAt.Year)
                && concert1.CreatedAt.Month.Equals(concert.CreatedAt.Month));
        }

        [TestMethod]
        public void UpdateEntityValuesSuccessfully()
        {
            var repository = new ConcertRepository(dbContext);
            var newName = "10 Años de Buitres";

            concert.TourName = newName;

            repository.Update(concert);

            var updatedEvent = repository.Get(concert.Id);

            Assert.IsTrue(updatedEvent.TourName.Equals(newName));
        }

        [TestMethod]
        public void UpdateEntityValuesNullThenShouldMantainPreviousValues()
        {
            var repository = new ConcertRepository(dbContext);

            repository.Update(new ConcertEntity { Id = concert.Id, TourName = null });

            var updatedEvent = repository.Get(concert.Id);


            Assert.IsTrue(updatedEvent.TourName.Equals(concert.TourName));
        }
    }
}
