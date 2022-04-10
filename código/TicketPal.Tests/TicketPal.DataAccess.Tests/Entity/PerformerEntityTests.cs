using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TicketPal.DataAccess.Entity;
using TicketPal.Domain.Constants;

namespace TicketPal.DataAccess.Tests.Entity
{
    [TestClass]
    public class PerformerEntityTests
    {
        private PerformerEntity performerEntity;
        private int id;
        private PerformerType performerTypeEnum;
        private string name;
        private DateTime startYear;
        private GenreEntity genre;
        //Se comentan estas variables mientras se resuelven detalles de implementación
        //de la clase PerformerEntity

        //private PerformerEntity artist;
        //private List<object> artists;


        [TestInitialize]
        public void Initialize()
        {
            performerEntity = new PerformerEntity();
            id = 1;
            performerTypeEnum = PerformerType.SOLO_ARTIST;
            name = "Roberto Carlos";
            startYear = new DateTime(1958, 01, 01);
            genre = new GenreEntity()
            {
                Id = 1,
                GenreName = "BossaNova"
            };
        }

        [TestMethod]
        public void SetPerformerTest()
        {
            performerEntity.Id = id;
            performerEntity.PerformerType = performerTypeEnum;
            performerEntity.Name = name;
            performerEntity.StartYear = startYear;
            performerEntity.Genre = genre;

            Assert.AreEqual(performerEntity.Id, id);
            Assert.AreEqual(performerEntity.PerformerType, performerTypeEnum);
            Assert.AreEqual(performerEntity.Name, name);
            Assert.AreEqual(performerEntity.StartYear, startYear);
            Assert.AreEqual(performerEntity.Genre, genre);
        }

        [TestMethod]
        public void GetPerformerTest()
        {
            int id = performerEntity.Id;
            PerformerType performerType = performerEntity.PerformerType;
            string name = performerEntity.Name;
            DateTime startYear = performerEntity.StartYear;
            BaseEntity genre = performerEntity.Genre;


            Assert.AreEqual(performerEntity.Id, id);
            Assert.AreEqual(performerEntity.PerformerType, performerType);
            Assert.AreEqual(performerEntity.Name, name);
            Assert.AreEqual(performerEntity.StartYear, startYear);
            Assert.AreEqual(performerEntity.Genre, genre);
        }
    }
}
