using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TicketPal.DataAccess.Tests.Entity
{
    [TestClass]
    public class BaseEntityTests
    {
        private BaseEntity baseEntity;
        private int Id;
        private DateTime createdAt;
        private DateTime updatedAt;

        [TestInitialize]
        public void Initialize()
        {
            Id = 1;
            createdAt = new DateTime(2022, 04, 06);
            updatedAt = new DateTime(2022, 04, 07);
            baseEntity = new BaseEntity();
        }

        [TestMethod]
        public void SetBaseEntityTest()
        {
            int Id = 2;
            DateTime createdAt = new DateTime(2022, 04, 07);
            DateTime updatedAt = new DateTime(2022, 04, 07);
            baseEntity.Id = Id;
            baseEntity.CreatedAt = createdAt;
            baseEntity.UpdatedAt = updatedAt;

            Assert.AreEqual(baseEntity.Id, Id);
            Assert.AreEqual(baseEntity.CreatedAt, createdAt);
            Assert.AreEqual(baseEntity.UpdatedAt, updatedAt);
        }

        [TestMethod]
        public void GetBaseEntityTest()
        {
            int Id = baseEntity.Id;
            DateTime createdAt = baseEntity.CreatedAt;
            DateTime updatedAt = baseEntity.UpdatedAt;

            Assert.AreEqual(baseEntity.Id, Id);
            Assert.AreEqual(baseEntity.CreatedAt, createdAt);
            Assert.AreEqual(baseEntity.UpdatedAt, updatedAt);
        }
    }
}
