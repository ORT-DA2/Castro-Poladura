using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Entity;

namespace TicketPal.DataAccess.Tests.Entity
{
    [TestClass]
    public class TicketEntityTests
    {
        private TicketEntity ticket;
        private UserEntity buyer;
        private DateTime purchaseDate;
        private ConcertEntity concert;
        private PerformerEntity band;
        private List<string> artists;
        private TicketStatus status;

        [TestInitialize]
        public void Initialize()
        {
            ticket = new TicketEntity();

            buyer = new UserEntity()
            {
                Id = 1,
                Firstname = "Susan",
                Lastname = "White",
                Email = "swhite@gmail.com"
            };

            purchaseDate = new DateTime(2022, 04, 04);

            artists = new List<string>();
            artists.Add("Freddie Mercury");
            artists.Add("Brian May");
            artists.Add("John Deacon");
            artists.Add("Roger Taylor");

            band = new PerformerEntity
            {
                PerformerType = PerformerType.BAND,
                Name = "Queen",
                StartYear = "1970",
                Genre = new GenreEntity
                {
                    Id = 2,
                    GenreName = "Rock"
                },
                Artists = artists
            };
            concert = new ConcertEntity
            {
                Id = 5,
                CreatedAt = new DateTime(2022, 01, 13),
                Date = new DateTime(2022, 11, 21),
                AvailableTickets = 1981,
                TicketPrice = 650,
                CurrencyType = CurrencyType.USD,
                EventType = EventType.CONCERT,
                TourName = "A Kind of Magic",
                Artist = band
            };
            status = TicketStatus.PURCHASED;
        }

        [TestMethod]
        public void GetTicketTest()
        {
            int idTicket = 2;
            ticket.Id = idTicket;
            ticket.Buyer = buyer;
            ticket.Status = status;
            ticket.PurchaseDate = purchaseDate;
            ticket.ShowName = concert.TourName;
            ticket.Event = concert;

            Assert.AreEqual(ticket.Id, idTicket);
            Assert.AreEqual(ticket.Buyer.Id, buyer.Id);
            Assert.AreEqual(ticket.Status, status);
            Assert.AreEqual(ticket.PurchaseDate, purchaseDate);
            Assert.AreEqual(ticket.ShowName, concert.TourName);
            Assert.AreEqual(ticket.Event.Id, concert.Id);
        }
    }
}
