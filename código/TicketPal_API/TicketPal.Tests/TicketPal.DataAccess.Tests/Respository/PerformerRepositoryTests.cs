using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketPal.DataAccess.Repository;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Exceptions;

namespace TicketPal.DataAccess.Tests.Respository
{
    [TestClass]
    public class PerformerRepositoryTests : RepositoryBaseConfigTests
    {
        private GenreEntity genre;
        private PerformerEntity performer;
        private List<PerformerEntity> members;

        [TestInitialize]
        public async Task Init()
        {
            genre = new GenreEntity()
            {
                Name = "Electropop"
            };

            members = new List<PerformerEntity> {
                new PerformerEntity {
                    UserInfo = new UserEntity { Firstname = "SomeName1" },
                    StartYear = "1987",
                    PerformerType = Constants.PERFORMER_TYPE_BAND,
                    Genre = genre,
                    Members = new List<PerformerEntity>()
                },
                new PerformerEntity {
                    UserInfo = new UserEntity { Firstname = "SomeName2" },
                    StartYear = "2000",
                    PerformerType = Constants.PERFORMER_TYPE_BAND,
                    Genre = genre,
                    Members = new List<PerformerEntity>()
                }
            };

            performer = new PerformerEntity
            {
                UserInfo = new UserEntity { Firstname = "SomeName" },
                StartYear = "1999",
                PerformerType = Constants.PERFORMER_TYPE_BAND,
                Genre = genre,
                Members = members
            };

            var repository = new PerformerRepository(dbContext);
            await repository.Add(performer);

        }

        [TestMethod]
        public async Task SavePerformerSuccessfully()
        {
            var repository = new PerformerRepository(dbContext);
            Assert.IsTrue((await repository.GetAll()).FirstOrDefault().UserInfo.Firstname == "SomeName");
        }

        [TestMethod]
        [ExpectedException(typeof(RepositoryException))]
        public async Task SaveDuplicatePerformersShouldThrowException()
        {
            var repository = new PerformerRepository(dbContext);
            await repository.Add(performer);
            await repository.Add(performer);
        }


        [TestMethod]
        public async Task ClearRepositoryShouldReturnCountCero()
        {
            var repository = new PerformerRepository(dbContext);
            Assert.IsTrue((await repository.GetAll()).Count == 3);
            repository.Clear();
            Assert.IsTrue((await repository.GetAll()).Count == 0);
        }

        [TestMethod]
        public void ExistPerformerByIdReturnsTrue()
        {
            var repository = new PerformerRepository(dbContext);
            Assert.IsTrue(repository.Exists(performer.Id));
        }

        [TestMethod]
        public async Task DeletedPerformerShouldNotExist()
        {
            var repository = new PerformerRepository(dbContext);
            await repository.Delete(performer.Id);
            Assert.IsFalse(repository.Exists(performer.Id));
        }

        [TestMethod]
        [ExpectedException(typeof(RepositoryException))]
        public async Task DeleteNonExistentPerformerThrowsException()
        {
            var repository = new PerformerRepository(dbContext);
            repository.Clear();
            await repository.Delete(performer.Id);
        }

        [TestMethod]
        public async Task SearchPerformerByNameTestCorrect()
        {
            var repository = new PerformerRepository(dbContext);
            PerformerEntity found = await repository.Get(g => g.UserInfo.Firstname.Equals(performer.UserInfo.Firstname));

            Assert.IsNotNull(found);
            Assert.AreEqual(performer.UserInfo.Firstname, found.UserInfo.Firstname);
        }

        [TestMethod]
        public async Task ShouldCreatedDateBeTheSameDay()
        {

            string performerName1 = "Bob Marley";
            string startYear1 = "1960";

            var performer1 = new PerformerEntity
            {
                UserInfo = new UserEntity { Firstname = performerName1 },
                StartYear = startYear1,
                PerformerType = Constants.PERFORMER_TYPE_SOLO_ARTIST,
                Genre = genre
            };

            var repository = new PerformerRepository(dbContext);
            await repository.Add(performer1);
            Assert.IsTrue(
                performer1.CreatedAt.Day.Equals(performer1.CreatedAt.Day)
                && performer1.CreatedAt.Year.Equals(performer1.CreatedAt.Year)
                && performer1.CreatedAt.Month.Equals(performer1.CreatedAt.Month));
        }

        [TestMethod]
        public async Task UpdateEntityValuesSuccessfully()
        {
            var repository = new PerformerRepository(dbContext);
            string newName = "Annie Lennox";

            performer.UserInfo.Firstname = newName;

            repository.Update(performer);
            var updatedPerformer = await repository.Get(performer.Id);

            Assert.IsTrue(updatedPerformer.UserInfo.Firstname.Equals(newName));
        }

        [TestMethod]
        public async Task UpdateEntityValuesNullThenShouldMantainPreviousValues()
        {
            var repository = new PerformerRepository(dbContext);
            repository.Update(new PerformerEntity { Id = performer.Id, UserInfo = null });

            var updatedPerformer = await repository.Get(performer.Id);

            Assert.IsNotNull(updatedPerformer.UserInfo);
        }
    }
}
