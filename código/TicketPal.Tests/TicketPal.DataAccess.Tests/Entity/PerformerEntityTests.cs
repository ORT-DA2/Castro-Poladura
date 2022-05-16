using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Entity;

namespace TicketPal.DataAccess.Tests.Entity
{
    [TestClass]
    public class PerformerEntityTests
    {
        private PerformerEntity performerEntity;
        private int id;
        private string performerTypeEnum;
        private string name;
        private string startDate;
        private GenreEntity genre;
        private string artists;


        [TestInitialize]
        public void Initialize()
        {
            performerEntity = new PerformerEntity();
            int idTicket = 1;
            id = idTicket;
            performerTypeEnum = Constants.PERFORMER_TYPE_SOLO_ARTIST;
            name = "Roberto Carlos";
            startDate = "1958";
            genre = new GenreEntity()
            {
                Id = 1,
                GenreName = "BossaNova"
            };
        }

        [TestMethod]
        public void GetSoloArtistPerformerTest()
        {
            performerEntity.Id = id;
            performerEntity.PerformerType = performerTypeEnum;
            performerEntity.Name = name;
            performerEntity.StartYear = startDate;
            performerEntity.Genre = genre;

            Assert.AreEqual(performerEntity.Id, id);
            Assert.AreEqual(performerEntity.PerformerType, performerTypeEnum);
            Assert.AreEqual(performerEntity.Name, name);
            Assert.AreEqual(performerEntity.StartYear, startDate);
            Assert.AreEqual(performerEntity.Genre, genre);
        }

        [TestMethod]
        public void GetBandPerformerTest()
        {
            int id = 77;
            var performerType = Constants.PERFORMER_TYPE_BAND;
            string name = "Ataque77";
            string startYear = "1987";
            genre.GenreName = "Punk Rock";

            artists = "Mariano Gabriel Martínez|Luciano Scaglione|Leonardo De Cecco|Martín Locarnini";

            performerEntity.Id = id;
            performerEntity.PerformerType = performerType;
            performerEntity.Name = name;
            performerEntity.StartYear = startYear;
            performerEntity.Genre = genre;
            performerEntity.Artists = artists;

            Assert.AreEqual(performerEntity.Id, id);
            Assert.AreEqual(performerEntity.PerformerType, performerType);
            Assert.AreEqual(performerEntity.Name, name);
            Assert.AreEqual(performerEntity.StartYear, startYear);
            Assert.AreEqual(performerEntity.Genre, genre);
            Assert.AreEqual(performerEntity.Artists, artists);
        }

    }
}
