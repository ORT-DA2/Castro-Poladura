﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public void Init()
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
            repository.Add(performer);

        }

        [TestMethod]
        public void SavePerformerSuccessfully()
        {
            var repository = new PerformerRepository(dbContext);
            Assert.IsTrue(repository.GetAll().ToList().FirstOrDefault().UserInfo.Firstname == "SomeName");
        }

        [TestMethod]
        [ExpectedException(typeof(RepositoryException))]
        public void SaveDuplicatePerformersShouldThrowException()
        {
            var repository = new PerformerRepository(dbContext);
            repository.Add(performer);
            repository.Add(performer);
        }


        [TestMethod]
        public void ClearRepositoryShouldReturnCountCero()
        {
            var repository = new PerformerRepository(dbContext);
            Assert.IsTrue(repository.GetAll().ToList().Count == 3);
            repository.Clear();
            Assert.IsTrue(repository.GetAll().ToList().Count == 0);
        }

        [TestMethod]
        public void ExistPerformerByIdReturnsTrue()
        {
            var repository = new PerformerRepository(dbContext);
            Assert.IsTrue(repository.Exists(performer.Id));
        }

        [TestMethod]
        public void DeletedPerformerShouldNotExist()
        {
            var repository = new PerformerRepository(dbContext);
            repository.Delete(performer.Id);
            Assert.IsFalse(repository.Exists(performer.Id));
        }

        [TestMethod]
        [ExpectedException(typeof(RepositoryException))]
        public void DeleteNonExistentPerformerThrowsException()
        {
            var repository = new PerformerRepository(dbContext);
            repository.Clear();
            repository.Delete(performer.Id);
        }

        [TestMethod]
        public void SearchPerformerByNameTestCorrect()
        {
            var repository = new PerformerRepository(dbContext);
            PerformerEntity found = repository.Get(g => g.UserInfo.Firstname.Equals(performer.UserInfo.Firstname));

            Assert.IsNotNull(found);
            Assert.AreEqual(performer.UserInfo.Firstname, found.UserInfo.Firstname);
        }

        [TestMethod]
        public void ShouldCreatedDateBeTheSameDay()
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
            repository.Add(performer1);
            Assert.IsTrue(
                performer1.CreatedAt.Day.Equals(performer1.CreatedAt.Day)
                && performer1.CreatedAt.Year.Equals(performer1.CreatedAt.Year)
                && performer1.CreatedAt.Month.Equals(performer1.CreatedAt.Month));
        }

        [TestMethod]
        public void UpdateEntityValuesSuccessfully()
        {
            var repository = new PerformerRepository(dbContext);
            string newName = "Annie Lennox";

            performer.UserInfo.Firstname = newName;

            repository.Update(performer);
            var updatedPerformer = repository.Get(performer.Id);

            Assert.IsTrue(updatedPerformer.UserInfo.Firstname.Equals(newName));
        }

        [TestMethod]
        public void UpdateEntityValuesNullThenShouldMantainPreviousValues()
        {
            var repository = new PerformerRepository(dbContext);
            repository.Update(new PerformerEntity { Id = performer.Id, UserInfo = null });

            var updatedPerformer = repository.Get(performer.Id);

            Assert.IsNotNull(updatedPerformer.UserInfo);
        }
    }
}
