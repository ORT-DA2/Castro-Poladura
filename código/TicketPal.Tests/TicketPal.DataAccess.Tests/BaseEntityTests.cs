using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TicketPal.DataAccess.Tests
{
    [TestClass]
    public class BaseEntityTests
    {
        private BaseEntity baseEntity;
        private int idBaseEntity;
        private DateTime createdAt;
        private DateTime updatedAt;

        [TestInitialize]
        public void Initialize()
        {
            idBaseEntity = 1;
            createdAt = new DateTime(2022, 04, 06);
            updatedAt = new DateTime(2022, 04, 07);
            baseEntity = new BaseEntity();
            baseEntity.IdBaseEntity = idBaseEntity;
            baseEntity.CreatedAt = createdAt;
            baseEntity.UpdatedAt = updatedAt;
        }

        [TestMethod]
        public void SetBaseEntityTest()
        {
            int idBaseEntity = 2;
            DateTime createdAt = new DateTime(2022, 04, 07);
            DateTime updatedAt = new DateTime(2022, 04, 07);
            baseEntity.IdBaseEntity = idBaseEntity;
            baseEntity.CreatedAt = createdAt;
            baseEntity.UpdatedAt = updatedAt;

            Assert.AreEqual(baseEntity.IdBaseEntity, idBaseEntity);
            Assert.AreEqual(baseEntity.CreatedAt, createdAt);
            Assert.AreEqual(baseEntity.UpdatedAt, updatedAt);
        }

        [TestMethod]
        public void GetBaseEntityTest()
        {
            int idBaseEntity = baseEntity.IdBaseEntity;
            DateTime createdAt = baseEntity.CreatedAt;
            DateTime updatedAt = baseEntity.UpdatedAt;

            Assert.AreEqual(baseEntity.IdBaseEntity, idBaseEntity);
            Assert.AreEqual(baseEntity.CreatedAt, createdAt);
            Assert.AreEqual(baseEntity.UpdatedAt, updatedAt);
        }
    }
}
