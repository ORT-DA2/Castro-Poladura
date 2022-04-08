using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TicketPal.Interfaces.Enumerations;

namespace TicketPal.DataAccess.Tests
{
    [TestClass]
    public class ConcertEntityTests
    {
        private ConcertEntity concert;
        private int idEvent;
        private DateTime eventDate;
        private int availableTickets;
        private decimal ticketPrice;
        private string tourName;
        private PerformerEntity artist;


        [TestInitialize]
        public void Initialize()
        {
            concert = new ConcertEntity();
            idEvent = 81;
            eventDate = new DateTime(2022, 08, 08);
            availableTickets = 1384;
            ticketPrice = 1345;
            tourName = "Carrousell";
            artist = new PerformerEntity()
            {
                PerformerType = PerformerTypeEnumeration.SOLO_ARTIST,    
                IdPerformer = 24,
                Name = "Tom Collins",
                StartYear = new DateTime(1999, 10, 03),
                Genre = new GenreEntity()
                {
                    IdGenre = 7,
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
        }
    }
}
