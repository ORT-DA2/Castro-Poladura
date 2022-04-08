using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TicketPal.Interfaces.Enumerations;

namespace TicketPal.DataAccess.Tests
{
    [TestClass]
    public class PerformerEntityTests
    {
        private PerformerEntity performerEntity;
        private int idPerformer;
        private PerformerTypeEnumeration performerTypeEnum;
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
            idPerformer = 1;
            performerTypeEnum = PerformerTypeEnumeration.SOLO_ARTIST;
            name = "Roberto Carlos";
            startYear = new DateTime(1958, 01, 01);
            genre = new GenreEntity()
            {
                IdGenre = 1,
                GenreName = "BossaNova"
            };
        }

        [TestMethod]
        public void SetPerformerTest()
        {
            performerEntity.IdPerformer = idPerformer;
            performerEntity.PerformerType = performerTypeEnum;
            performerEntity.Name = name;
            performerEntity.StartYear = startYear;
            performerEntity.Genre = genre;

            Assert.AreEqual(performerEntity.IdPerformer, idPerformer);
            Assert.AreEqual(performerEntity.PerformerType, performerTypeEnum);
            Assert.AreEqual(performerEntity.Name, name);
            Assert.AreEqual(performerEntity.StartYear, startYear);
            Assert.AreEqual(performerEntity.Genre, genre);
        }

        [TestMethod]
        public void GetPerformerTest()
        {
            int idPerformer = performerEntity.IdPerformer;
            PerformerTypeEnumeration performerType = performerEntity.PerformerType;
            string name = performerEntity.Name;
            DateTime startYear = performerEntity.StartYear;
            GenreEntity genre = performerEntity.Genre;


            Assert.AreEqual(performerEntity.IdPerformer, idPerformer);
            Assert.AreEqual(performerEntity.PerformerType, performerType);
            Assert.AreEqual(performerEntity.Name, name);
            Assert.AreEqual(performerEntity.StartYear, startYear);
            Assert.AreEqual(performerEntity.Genre, genre);
        }
    }
}
