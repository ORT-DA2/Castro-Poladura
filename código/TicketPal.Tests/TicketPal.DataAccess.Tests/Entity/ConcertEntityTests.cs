using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TicketPal.DataAccess.Entity;
using TicketPal.Domain.Constants;


namespace TicketPal.DataAccess.Tests.Entity
{
    [TestClass]
    public class ConcertEntityTests
    {
        private ConcertEntity concert;
        private int id;
        private DateTime createdAt;
        private DateTime updatedAt;
        private int idEvent;
        private DateTime eventDate;
        private int availableTickets;
        private decimal ticketPrice;
        private string tourName;
        private PerformerEntity artist;


        [TestInitialize]
        public void Initialize()
        {
            id = 1;
            createdAt = new DateTime(2022, 04, 06);
            updatedAt = new DateTime(2022, 04, 07);
            concert = new ConcertEntity();
            idEvent = 81;
            eventDate = new DateTime(2022, 08, 08);
            availableTickets = 1384;
            ticketPrice = 1345;
            tourName = "Carrousell";
            artist = new PerformerEntity()
            {
                PerformerType = PerformerType.SOLO_ARTIST,
                Id = 24,
                Name = "Tom Collins",
                StartYear = new DateTime(1999, 10, 03),
                Genre = new GenreEntity()
                {
                    Id = 7,
                    GenreName = "Rock"
                },
            };
        }

        [TestMethod]
        public void SetConcertTest()
        {
            concert.IdEvent = idEvent;
            concert.EventDate = eventDate;
            concert.AvailableTickets = availableTickets;
            concert.TicketPrice = ticketPrice;
            concert.TourName = tourName;
            concert.Artist = artist;
            concert.Id = id;
            concert.CreatedAt = createdAt;
            concert.UpdatedAt = updatedAt;

            Assert.AreEqual(concert.IdEvent, idEvent);
            Assert.AreEqual(concert.EventDate, eventDate);
            Assert.AreEqual(concert.AvailableTickets, availableTickets);
            Assert.AreEqual(concert.TicketPrice, ticketPrice);
            Assert.AreEqual(concert.TourName, tourName);
            Assert.AreEqual(concert.Artist, artist);
        }

        [TestMethod]
        public void GetConcertTest()
        {
            int idEvent = concert.IdEvent;
            DateTime eventDate = concert.EventDate;
            int availableTicket = concert.AvailableTickets;
            decimal ticketPrice = concert.TicketPrice;
            string tourName = concert.TourName;
            PerformerEntity artist = concert.Artist;

            Assert.AreEqual(concert.IdEvent, idEvent);
            Assert.AreEqual(concert.EventDate, eventDate);
            Assert.AreEqual(concert.AvailableTickets, availableTickets);
            Assert.AreEqual(concert.TicketPrice, ticketPrice);
            Assert.AreEqual(concert.TourName, tourName);
            Assert.AreEqual(concert.Artist, artist);
            Assert.AreEqual(concert.Id, id);
            Assert.AreEqual(concert.CreatedAt, createdAt);
            Assert.AreEqual(concert.UpdatedAt, updatedAt);
        }
    }
}
