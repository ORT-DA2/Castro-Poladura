﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
        private CurrencyType currency;
        private string tourName;
        private PerformerEntity artist;
        private EventType eventType;

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
            currency = CurrencyType.UYU; 
            tourName = "Carrousell";
            eventType = EventType.CONCERT;
            artist = new PerformerEntity()
            {
                PerformerType = PerformerType.SOLO_ARTIST,
                Id = 24,
                Name = "Tom Collins",
                StartYear = "1999",
                Genre = new GenreEntity()
                {
                    Id = 7,
                    GenreName = "Rock"
                }
            };
            concert.Id = id;
            concert.EventType = eventType;
            concert.Date = eventDate;
            concert.AvailableTickets = availableTickets;
            concert.TicketPrice = ticketPrice;
            concert.CurrencyType = currency;
            concert.TourName = tourName;
            concert.Artist = artist;
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
            Assert.AreEqual(concert.Artist, artist);
        }
    }
}
