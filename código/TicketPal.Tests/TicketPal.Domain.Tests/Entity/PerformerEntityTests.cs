using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TicketPal.Domain.Entity;

namespace TicketPal.Domain.Tests.Entity
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

        [TestInitialize]
        public void Initialize()
        {
            performerEntity = new PerformerEntity();
            int idTicket = 1;
            id = idTicket;
            performerTypeEnum = Constants.Constants.PERFORMER_TYPE_SOLO_ARTIST;
            name = "Roberto Carlos";
            startDate = "1958";
            genre = new GenreEntity()
            {
                Id = 1,
                Name = "BossaNova"
            };
        }

        [TestMethod]
        public void GetSoloArtistPerformerTest()
        {
            performerEntity.Id = id;
            performerEntity.PerformerType = performerTypeEnum;
            performerEntity.UserInfo = new UserEntity { Firstname = name };
            performerEntity.StartYear = startDate;
            performerEntity.Genre = genre;

            Assert.AreEqual(performerEntity.Id, id);
            Assert.AreEqual(performerEntity.PerformerType, performerTypeEnum);
            Assert.AreEqual(performerEntity.UserInfo.Firstname, name);
            Assert.AreEqual(performerEntity.StartYear, startDate);
            Assert.AreEqual(performerEntity.Genre, genre);
        }

        [TestMethod]
        public void GetBandPerformerTest()
        {
            int id = 77;
            var performerType = Constants.Constants.PERFORMER_TYPE_BAND;
            string name = "Ataque77";
            string startYear = "1987";
            genre.Name = "Punk Rock";

            performerEntity.Id = id;
            performerEntity.PerformerType = performerType;
            performerEntity.UserInfo = new UserEntity { Firstname = name };
            performerEntity.StartYear = startYear;
            performerEntity.Genre = genre;
            performerEntity.Members = new List<UserEntity>();

            Assert.AreEqual(performerEntity.Id, id);
            Assert.AreEqual(performerEntity.PerformerType, performerType);
            Assert.AreEqual(performerEntity.UserInfo.Firstname, name);
            Assert.AreEqual(performerEntity.StartYear, startYear);
            Assert.AreEqual(performerEntity.Genre, genre);
        }

    }
}
