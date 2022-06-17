using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicketPal.BusinessLogic.Mapper;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Models.Response;

namespace TicketPal.BusinessLogic.Tests.Mapper
{
    [TestClass]
    public class PerformerMappingTest
    {
        [TestMethod]
        public void PerformerEntityToPerformerMapperTest()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapping>());
            var mapper = config.CreateMapper();

            var toBeMapped = new PerformerEntity
            {
                Id = 1,
                UserInfo = new UserEntity { Id = 1, Firstname = "someName" },
                Members = new List<PerformerEntity>(),
                Genre = new GenreEntity { Name = "someGenre" },
                PerformerType = Constants.PERFORMER_TYPE_BAND,
                StartYear = "1968"
            };

            var performer = mapper.Map<Performer>(toBeMapped);

            Assert.IsNotNull(performer);
            Assert.IsTrue(performer.StartYear.Equals(toBeMapped.StartYear)
                && performer.UserInfo.Firstname.Equals(toBeMapped.UserInfo.Firstname)
                && performer.PerformerType.Equals(toBeMapped.PerformerType)
                && performer.Id == toBeMapped.Id
            );
            CollectionAssert.AreEquivalent(toBeMapped.Members, performer.Members.ToList());
        }
    }
}