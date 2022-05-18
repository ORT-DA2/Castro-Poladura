﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Entity;

namespace TicketPal.DataAccess.Tests.Entity
{
    [TestClass]
    public class ConcertEntityTests
    {
        private ConcertEntity concert;
        private int id;
        private DateTime createdAt;
        private DateTime updatedAt;
        private DateTime eventDate;
        private int availableTickets;
        private decimal ticketPrice;
        private string currency;
        private string tourName;
        private IEnumerable<PerformerEntity> artists;
        private string eventType;

        [TestInitialize]
        public void Initialize()
        {
            id = 1;
            createdAt = new DateTime(2022, 04, 06);
            updatedAt = new DateTime(2022, 04, 07);
            concert = new ConcertEntity();
            eventDate = new DateTime(2022, 08, 08);
            availableTickets = 1384;
            ticketPrice = 1345;
            currency = Constants.CURRENCY_URUGUAYAN_PESO; 
            tourName = "Carrousell";
            eventType = Constants.EVENT_CONCERT_TYPE;
            artists = new List<PerformerEntity> {
                new PerformerEntity()
                {
                    PerformerType = Constants.PERFORMER_TYPE_SOLO_ARTIST,
                    Id = 24,
                    UserInfo = new UserEntity { Firstname = "Tim", Lastname = "Collins"},
                    StartYear = "1999",
                    Genre = new GenreEntity()
                    {
                        Id = 7,
                        GenreName = "Rock"
                    }
                }
            };
            concert.Id = id;
            concert.EventType = eventType;
            concert.Date = eventDate;
            concert.AvailableTickets = availableTickets;
            concert.TicketPrice = ticketPrice;
            concert.CurrencyType = currency;
            concert.TourName = tourName;
            concert.Artists = artists;
            concert.Id = id;
            concert.CreatedAt = createdAt;
            concert.UpdatedAt = updatedAt;
        }

        [TestMethod]
        public void GetConcertTest()
        {

            Assert.AreEqual(concert.Id, id);
            Assert.AreEqual(concert.EventType, eventType);
            Assert.AreEqual(concert.Date, eventDate);
            Assert.AreEqual(concert.AvailableTickets, availableTickets);
            Assert.AreEqual(concert.TicketPrice, ticketPrice);
            Assert.AreEqual(concert.CurrencyType, currency);
            Assert.AreEqual(concert.TourName, tourName);
            CollectionAssert.AreEqual(concert.Artists.ToList(), artists.ToList());
        }
    }
}
