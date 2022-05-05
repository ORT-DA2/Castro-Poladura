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
    public class ConcertRepositoryTests : RepositoryBaseConfigTests
    {
        [TestMethod]
        public void SaveEventSuccessfully()
        {
            string tourName = "Pies Descalzos";
            PerformerEntity performer = new PerformerEntity()
            {
                Id = 1,
                Name = "Shakira",
                StartYear = "1990",
                PerformerType = PerformerType.SOLO_ARTIST,
                Genre = new GenreEntity()
                {
                    Id = 1,
                    GenreName = "Pop"
                },
            };

            var concert = new ConcertEntity()
            {
                Id = 1,
                Artist = performer,
                AvailableTickets = 500,
                CurrencyType = CurrencyType.USD,
                Date = DateTime.Now,
                EventType = EventType.CONCERT,
                TicketPrice = 100,
                TourName = tourName
            };


            var repository = new ConcertRepository(dbContext);
            repository.Add(concert);

            Assert.IsTrue(repository.GetAll().ToList().FirstOrDefault().TourName == tourName);
        }

        [TestMethod]
        [ExpectedException(typeof(RepositoryException))]
        public void SaveDuplicateEventsShouldThrowException()
        {
            PerformerEntity performer = new PerformerEntity()
            {
                Id = 1,
                Name = "Michael Jackson",
                StartYear = "1964",
                PerformerType = PerformerType.SOLO_ARTIST,
                Genre = new GenreEntity()
                {
                    Id = 1,
                    GenreName = "Pop"
                },
            };

            var concert1 = new ConcertEntity()
            {
                Id = 1,
                Artist = performer,
                AvailableTickets = 235,
                CurrencyType = CurrencyType.USD,
                Date = DateTime.Now,
                EventType = EventType.CONCERT,
                TicketPrice = 300,
                TourName = "Smooth Criminal"
            };

            var concert2 = new ConcertEntity()
            {
                Id = 1,
                Artist = performer,
                AvailableTickets = 235,
                CurrencyType = CurrencyType.USD,
                Date = DateTime.Now,
                EventType = EventType.CONCERT,
                TicketPrice = 300,
                TourName = "Smooth Criminal"
            };

            var repository = new ConcertRepository(dbContext);
            repository.Add(concert1);
            repository.Add(concert2);
        }


        [TestMethod]
        public void ClearRepositoryShouldReturnCountCero()
        {
            PerformerEntity performer = new PerformerEntity()
            {
                Id = 1,
                Name = "Robbie Williams",
                StartYear = "1990",
                PerformerType = PerformerType.SOLO_ARTIST,
                Genre = new GenreEntity()
                {
                    Id = 1,
                    GenreName = "Pop rock"
                },
            };

            var concert = new ConcertEntity()
            {
                Id = 1,
                Artist = performer,
                AvailableTickets = 2156,
                CurrencyType = CurrencyType.USD,
                Date = DateTime.Now,
                EventType = EventType.CONCERT,
                TicketPrice = 160,
                TourName = "Rock DJ"
            };

            var repository = new ConcertRepository(dbContext);
            repository.Add(concert);
            repository.Clear();

            Assert.IsTrue(repository.GetAll().ToList().Count == 0);
        }

        [TestMethod]
        public void ShouldRepositoryBeEmptyWhenEventDeleted()
        {
            string artists = "Kurt Cobain|Krist Novoselic|Dave Grohl";

            PerformerEntity performer = new PerformerEntity()
            {
                Id = 1,
                Name = "Nirvana",
                StartYear = "1987",
                PerformerType = PerformerType.BAND,
                Genre = new GenreEntity()
                {
                    Id = 1,
                    GenreName = "Grunge"
                },
                Artists = artists
            };

            var concert = new ConcertEntity()
            {
                Id = 1,
                Artist = performer,
                AvailableTickets = 235,
                CurrencyType = CurrencyType.USD,
                Date = DateTime.Now,
                EventType = EventType.CONCERT,
                TicketPrice = 300,
                TourName = "Bleach"
            };

            var repository = new ConcertRepository(dbContext);
            repository.Add(concert);
            repository.Delete(concert.Id);

            Assert.IsTrue(repository.IsEmpty());
        }

        [TestMethod]
        public void ExistEventByIdReturnsTrue()
        {
            PerformerEntity performer = new PerformerEntity()
            {
                Id = 1,
                Name = "Carrie Underwood",
                StartYear = "2005",
                PerformerType = PerformerType.SOLO_ARTIST,
                Genre = new GenreEntity()
                {
                    Id = 1,
                    GenreName = "Country"
                },
            };

            var concert = new ConcertEntity()
            {
                Id = 1,
                Artist = performer,
                AvailableTickets = 768,
                CurrencyType = CurrencyType.USD,
                Date = DateTime.Now,
                EventType = EventType.CONCERT,
                TicketPrice = 59.99m,
                TourName = "Some Hearts"
            };

            var repository = new ConcertRepository(dbContext);
            repository.Add(concert);

            Assert.IsTrue(repository.Exists(concert.Id));
        }

        [TestMethod]
        public void DeletedEventShouldNotExist()
        {
            PerformerEntity performer = new PerformerEntity()
            {
                Id = 1,
                Name = "Brad Paisley",
                StartYear = "1999",
                PerformerType = PerformerType.SOLO_ARTIST,
                Genre = new GenreEntity()
                {
                    Id = 1,
                    GenreName = "Country"
                },
            };

            var concert = new ConcertEntity()
            {
                Id = 1,
                Artist = performer,
                AvailableTickets = 3457,
                CurrencyType = CurrencyType.USD,
                Date = DateTime.Now,
                EventType = EventType.CONCERT,
                TicketPrice = 78,
                TourName = "The World"
            };

            var repository = new ConcertRepository(dbContext);
            repository.Add(concert);
            repository.Delete(concert.Id);

            Assert.IsFalse(repository.Exists(concert.Id));
        }

        [TestMethod]
        [ExpectedException(typeof(RepositoryException))]
        public void DeleteNonExistentEventThrowsException()
        {
            PerformerEntity performer = new PerformerEntity()
            {
                Id = 1,
                Name = "Jaime Roos",
                StartYear = "1977",
                PerformerType = PerformerType.SOLO_ARTIST,
                Genre = new GenreEntity()
                {
                    Id = 1,
                    GenreName = "Candombe"
                },
            };

            var concert = new ConcertEntity()
            {
                Id = 1,
                Artist = performer,
                AvailableTickets = 2165,
                CurrencyType = CurrencyType.UYU,
                Date = DateTime.Now,
                EventType = EventType.CONCERT,
                TicketPrice = 1500,
                TourName = "A las 10"
            };

            var repository = new ConcertRepository(dbContext);
            repository.Delete(concert.Id);
        }

        [TestMethod]
        public void SearchEventByNameTestCorrect()
        {
            PerformerEntity performer = new PerformerEntity()
            {
                Id = 1,
                Name = "Natalia Oreiro",
                StartYear = "1989",
                PerformerType = PerformerType.SOLO_ARTIST,
                Genre = new GenreEntity()
                {
                    Id = 1,
                    GenreName = "Pop latino"
                },
            };

            var concert = new ConcertEntity()
            {
                Id = 1,
                Artist = performer,
                AvailableTickets = 10000,
                CurrencyType = CurrencyType.UYU,
                Date = DateTime.Now,
                EventType = EventType.CONCERT,
                TicketPrice = 2300,
                TourName = "Turmalina"
            };

            var repository = new ConcertRepository(dbContext);
            repository.Add(concert);

            ConcertEntity found = repository.Get(g => g.TourName.Equals(concert.TourName));

            Assert.IsNotNull(found);
            Assert.AreEqual(concert.TourName, found.TourName);
        }

        [TestMethod]
        public void ShouldCreatedDateBeTheSameDay()
        {
            string artists = "Gustavo Parodi|Gabriel Peluffo|José Rambao|Orlando Fernández|Federico 'Kako' Bianco";

            PerformerEntity performer = new PerformerEntity()
            {
                Id = 1,
                Name = "Buitres",
                StartYear = "1989",
                PerformerType = PerformerType.BAND,
                Genre = new GenreEntity()
                {
                    Id = 1,
                    GenreName = "Rock and Roll"
                },
                Artists = artists
            };

            var concert1 = new ConcertEntity()
            {
                Id = 1,
                Artist = performer,
                AvailableTickets = 5624,
                CurrencyType = CurrencyType.UYU,
                Date = DateTime.Now,
                EventType = EventType.CONCERT,
                TicketPrice = 2000,
                TourName = "Buitres Después de la Una"
            };
            var concert2 = new ConcertEntity()
            {
                Id = 2,
                Artist = performer,
                AvailableTickets = 20000,
                CurrencyType = CurrencyType.UYU,
                Date = DateTime.Now.AddDays(30),
                EventType = EventType.CONCERT,
                TicketPrice = 2100,
                TourName = "El amor te ha hecho idiota"
            };

            var repository = new ConcertRepository(dbContext);
            repository.Add(concert1);
            repository.Add(concert2);
            Assert.IsTrue(
                concert1.CreatedAt.Day.Equals(concert2.CreatedAt.Day)
                && concert1.CreatedAt.Year.Equals(concert2.CreatedAt.Year)
                && concert1.CreatedAt.Month.Equals(concert2.CreatedAt.Month));
        }

        [TestMethod]
        public void UpdateEntityValuesSuccessfully()
        {
            string artists = "Gustavo Parodi|Gabriel Peluffo|José Rambao|Orlando Fernández|Federico 'Kako' Bianco";

            PerformerEntity performer = new PerformerEntity()
            {
                Id = 1,
                Name = "Buitres",
                StartYear = "1989",
                PerformerType = PerformerType.BAND,
                Genre = new GenreEntity()
                {
                    Id = 1,
                    GenreName = "Rock and Roll"
                },
                Artists = artists
            };

            var concert = new ConcertEntity()
            {
                Id = 1,
                Artist = performer,
                AvailableTickets = 5624,
                CurrencyType = CurrencyType.UYU,
                Date = DateTime.Now,
                EventType = EventType.CONCERT,
                TicketPrice = 2000,
                TourName = "Buitres Después de la Una"
            };

            var repository = new ConcertRepository(dbContext);
            repository.Add(concert);

            var newName = "10 Años de Buitres";

            concert.TourName = newName;

            repository.Update(concert);

            var updatedEvent = repository.Get(concert.Id);

            Assert.IsTrue(updatedEvent.TourName.Equals(newName));
        }

        [TestMethod]
        public void UpdateEntityValuesNullThenShouldMantainPreviousValues()
        {
            PerformerEntity performer = new PerformerEntity()
            {
                Id = 1,
                Name = "Jaime Roos",
                StartYear = "1977",
                PerformerType = PerformerType.SOLO_ARTIST,
                Genre = new GenreEntity()
                {
                    Id = 1,
                    GenreName = "Candombe"
                },
            };

            var concert = new ConcertEntity()
            {
                Id = 1,
                Artist = performer,
                AvailableTickets = 2165,
                CurrencyType = CurrencyType.UYU,
                Date = DateTime.Now,
                EventType = EventType.CONCERT,
                TicketPrice = 1500,
                TourName = "A las 10"
            };

            var repository = new ConcertRepository(dbContext);
            repository.Add(concert);

            repository.Update(new ConcertEntity { Id = concert.Id, TourName = null });

            var updatedEvent = repository.Get(concert.Id);


            Assert.IsTrue(updatedEvent.TourName.Equals(concert.TourName));
        }
    }
}
