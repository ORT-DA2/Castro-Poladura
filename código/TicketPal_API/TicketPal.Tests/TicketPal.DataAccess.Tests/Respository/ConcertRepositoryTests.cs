using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task Init()
        {
            var genre = new GenreEntity { Name = "Pop" };

            artists = new List<PerformerEntity> {
                new PerformerEntity {
                    UserInfo = new UserEntity { Firstname = "name1" },
                    PerformerType = Constants.PERFORMER_TYPE_SOLO_ARTIST,
                    StartYear = "1987",
                    Genre = genre,
                    Members = new List<PerformerEntity>()
                },
                new PerformerEntity {
                    UserInfo = new UserEntity { Firstname = "name2"},
                    PerformerType = Constants.PERFORMER_TYPE_SOLO_ARTIST,
                    StartYear = "2000",
                    Genre = genre,
                    Members = new List<PerformerEntity>()
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
            await repository.Add(concert);
        }
        [TestMethod]
        public async Task CheckSavedEvent()
        {
            var repository = new ConcertRepository(dbContext);
            Assert.IsTrue((await repository.GetAll()).ToList().FirstOrDefault().TourName == "someTour");
        }

        [TestMethod]
        [ExpectedException(typeof(RepositoryException))]
        public async Task SaveDuplicateEventsShouldThrowException()
        {
            var repository = new ConcertRepository(dbContext);
            await repository.Add(concert);
        }


        [TestMethod]
        public async Task ClearRepositoryShouldReturnCountCero()
        {
            var repository = new ConcertRepository(dbContext);

            Assert.IsTrue((await repository.GetAll()).ToList().Count == 1);
            repository.Clear();
            Assert.IsTrue((await repository.GetAll()).ToList().Count == 0);
        }

        [TestMethod]
        public async Task ShouldRepositoryBeEmptyWhenEventDeleted()
        {

            var repository = new ConcertRepository(dbContext);
            await repository.Delete(concert.Id);

            Assert.IsTrue(repository.IsEmpty());
        }

        [TestMethod]
        public void ExistEventByIdReturnsTrue()
        {
            var repository = new ConcertRepository(dbContext);
            Assert.IsTrue(repository.Exists(concert.Id));
        }

        [TestMethod]
        public async Task DeletedEventShouldNotExist()
        {
            var repository = new ConcertRepository(dbContext);
            await repository.Delete(concert.Id);

            Assert.IsFalse(repository.Exists(concert.Id));
        }

        [TestMethod]
        [ExpectedException(typeof(RepositoryException))]
        public async Task DeleteNonExistentEventThrowsException()
        {
            var repository = new ConcertRepository(dbContext);
            repository.Clear();
            await repository.Delete(concert.Id);
        }

        [TestMethod]
        public async Task SearchEventByNameTestCorrect()
        {
            var repository = new ConcertRepository(dbContext);
            ConcertEntity found = await repository.Get(g => g.TourName.Equals(concert.TourName));

            Assert.IsNotNull(found);
            Assert.AreEqual(concert.TourName, found.TourName);
        }

        [TestMethod]
        public async Task ShouldCreatedDateBeTheSameDay()
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
            await repository.Add(concert1);
            Assert.IsTrue(
                concert1.CreatedAt.Day.Equals(concert.CreatedAt.Day)
                && concert1.CreatedAt.Year.Equals(concert.CreatedAt.Year)
                && concert1.CreatedAt.Month.Equals(concert.CreatedAt.Month));
        }

        [TestMethod]
        public async Task UpdateEntityValuesSuccessfully()
        {
            var repository = new ConcertRepository(dbContext);
            var newName = "10 Años de Buitres";

            concert.TourName = newName;

            repository.Update(concert);

            var updatedEvent = await repository.Get(concert.Id);

            Assert.IsTrue(updatedEvent.TourName.Equals(newName));
        }

        [TestMethod]
        public async Task UpdateEntityValuesNullThenShouldMantainPreviousValues()
        {
            var repository = new ConcertRepository(dbContext);

            repository.Update(new ConcertEntity { Id = concert.Id, TourName = null });

            var updatedEvent = await repository.Get(concert.Id);


            Assert.IsTrue(updatedEvent.TourName.Equals(concert.TourName));
        }
    }
}
