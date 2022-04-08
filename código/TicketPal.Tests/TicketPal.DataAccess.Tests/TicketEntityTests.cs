using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using TicketPal.Interfaces.Enumerations;

namespace TicketPal.DataAccess.Tests
{
    [TestClass]
    public class TicketEntityTests
    {
        private TicketEntity ticket;
        private int idTicket;
        private User buyer;
        private TicketStatusEnumeration status;
        private DateTime purchaseDate;
        private string showName;
        private PerformerEntity artist;

        [TestInitialize]
        public void Initialize()
        {
            idTicket = 1;
            buyer = new User()
            {
                IdUser = 1,
                FirstName = "Susan",
                LastName = "White",
                Email = "swhite@gmail.com"
            };
            status = TicketStatusEnumeration.PURCHASED;
            purchaseDate = new DateTime(2022, 04, 04);
            showName = "Solo";
            artist = new PerformerEntity
            {
                PerformerType = PerformerTypeEnumeration.SOLO_ARTIST,
                Name = "Enrique Estilos",
                StartYear = new DateTime(2010, 01, 13),
                Genre = new GenreEntity
                {
                    IdGenre = 1,
                    GenreName = "Pop"
                }              
            };
            
        }

        [TestMethod]
        public void SetTicketTest()
        {
            PerformerEntity singer = new PerformerEntity
            {
                PerformerType = PerformerTypeEnumeration.SOLO_ARTIST,
                Name = "Freddie Mercury",
                StartYear = new DateTime(1969, 01, 01),
                Genre = new GenreEntity
                {
                    IdGenre = 2,
                    GenreName = "Rock"
                }
            };
            PerformerEntity guitar = new PerformerEntity
            {
                PerformerType = PerformerTypeEnumeration.SOLO_ARTIST,
                Name = "Brian May",
                StartYear = new DateTime(1965, 01, 01),
                Genre = new GenreEntity
                {
                    IdGenre = 2,
                    GenreName = "Rock"
                }
            };
            PerformerEntity bass = new PerformerEntity
            {
                PerformerType = PerformerTypeEnumeration.SOLO_ARTIST,
                Name = "John Deacon",
                StartYear = new DateTime(1965, 01, 01),
                Genre = new GenreEntity
                {
                    IdGenre = 2,
                    GenreName = "Rock"
                }
            };
            PerformerEntity battery = new PerformerEntity
            {
                PerformerType = PerformerTypeEnumeration.SOLO_ARTIST,
                Name = "Roger Taylor",
                StartYear = new DateTime(1968, 01, 01),
                Genre = new GenreEntity
                {
                    IdGenre = 2,
                    GenreName = "Rock"
                }
            };

            List<PerformerEntity> artistList = new List<PerformerEntity>();
            artistList.Add(guitar);
            artistList.Add(bass);
            artistList.Add(battery);
            artistList.Add(singer);


            int idTicket = 2;
            User buyer = new User()
            {
                IdUser = 2,
                FirstName = "Charles",
                LastName = "Green",
                Email = "cgreen@gmail.com"
            };
            TicketStatusEnumeration status = TicketStatusEnumeration.USED;
            DateTime purchaseDate = new DateTime(2022, 02, 15);
            string tourName = "A Kind of Magic";
            PerformerEntity artist = new PerformerEntity
            {
                PerformerType = PerformerTypeEnumeration.BAND,
                Name = "Queen",
                StartYear = new DateTime(1970, 04, 27),
                Genre = new GenreEntity
                {
                    IdGenre = 2,
                    GenreName = "Rock"
                },
                Artists = artistList
            };


            ticket.IdTicket = idTicket;
            ticket.Buyer = buyer;
            ticket.Status = status;
            ticket.PurchaseDate = purchaseDate;
            ticket.ShowName = tourName;
            ticket.Artist = artist;

            Assert.AreEqual(ticket.IdTicket, idTicket);
            Assert.AreEqual(ticket.Buyer.IdUser, buyer.IdUser);
            Assert.AreEqual(ticket.Status, status);
            Assert.AreEqual(ticket.PurchaseDate, purchaseDate);
            Assert.AreEqual(ticket.ShowName, tourName);
            Assert.AreEqual(ticket.Artist.Name, artist.Name);
        }

        [TestMethod]
        public void GetTicketTest()
        {
            int idTicket = ticket.IdTicket;
            User buyer = ticket.Buyer;
            TicketStatusEnumeration status = ticket.Status;
            DateTime purchaseDate = ticket.PurchaseDate;
            string tourName = ticket.ShowName;

            Assert.AreEqual(ticket.IdTicket, idTicket);
            Assert.AreEqual(ticket.Buyer.IdUser, buyer.IdUser);
            Assert.AreEqual(ticket.Status, status);
            Assert.AreEqual(ticket.PurchaseDate, purchaseDate);
            Assert.AreEqual(ticket.ShowName, tourName);

        }
    }
}
