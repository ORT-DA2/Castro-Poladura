using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TicketPal.Domain.Entity;

namespace TicketPal.Domain.Tests.Entity
{
    [TestClass]
    public class TicketEntityTests
    {
        private TicketEntity ticket;
        private UserEntity buyer;
        private DateTime purchaseDate;
        private ConcertEntity concert;
        private List<PerformerEntity> bands;
        private string status;
        private string code;

        [TestInitialize]
        public void SetUp()
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

            bands = new List<PerformerEntity> {
                new PerformerEntity
                {
                    PerformerType = Constants.Constants.PERFORMER_TYPE_BAND,
                    UserInfo = new UserEntity{ Firstname = "Queen"},
                    StartYear = "1970",
                    Genre = new GenreEntity
                    {
                        Id = 2,
                        Name = "Rock"
                    },
                    Members = new List<PerformerEntity>()
                }
            };

            concert = new ConcertEntity
            {
                Id = 5,
                CreatedAt = new DateTime(2022, 01, 13),
                Date = new DateTime(2022, 11, 21),
                AvailableTickets = 1981,
                TicketPrice = 650,
                CurrencyType = Constants.Constants.CURRENCY_US_DOLLARS,
                EventType = Constants.Constants.EVENT_CONCERT_TYPE,
                TourName = "A Kind of Magic",
                Artists = bands
            };
            status = Constants.Constants.TICKET_PURCHASED_STATUS;
            code = "p3q59gjnfjgo4uqfjDXNCLKMQP31foiqnvjdanv";

        }

        [TestMethod]
        public void GetTicketTest()
        {
            int idTicket = 2;
            ticket.Id = idTicket;
            ticket.Buyer = buyer;
            ticket.Status = status;
            ticket.PurchaseDate = purchaseDate;
            ticket.Code = code;
            ticket.Event = concert;

            Assert.AreEqual(ticket.Id, idTicket);
            Assert.AreEqual(ticket.Buyer.Id, buyer.Id);
            Assert.AreEqual(ticket.Status, status);
            Assert.AreEqual(ticket.PurchaseDate, purchaseDate);
            Assert.AreEqual(ticket.Code, code);
            Assert.AreEqual(ticket.Event.Id, concert.Id);
        }
    }
}
