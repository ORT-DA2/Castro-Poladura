﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        [TestMethod]
        public void SavePerformerSuccessfully()
        {
            string performerName = "Daft Punk";
            string startYear = "1993";
            PerformerType performerType = PerformerType.BAND;
            GenreEntity genre = new GenreEntity()
            {
                Id = 1,
                GenreName = "Electropop"
            };
            List<string> artists = new List<string>();
            artists.Add("Guy-Manuel de Homem-Christo");
            artists.Add("Thomas Bangalter");

            var performer = new PerformerEntity
            {
                Id = 1,
                Name = performerName,
                StartYear = startYear,
                PerformerType = performerType,
                Genre = genre,
                Artists = artists
            };

            var repository = new PerformerRepository(dbContext);
            repository.Add(performer);

            Assert.IsTrue(repository.GetAll().ToList().FirstOrDefault().Name == performerName);
        }

        [TestMethod]
        [ExpectedException(typeof(RepositoryException))]
        public void SaveDuplicatePerformersShouldThrowException()
        {
            string performerName = "Julian Casablancas";
            string startYear = "1998";
            PerformerType performerType = PerformerType.SOLO_ARTIST;
            GenreEntity genre = new GenreEntity()
            {
                Id = 1,
                GenreName = "Rock"
            };

            var performer1 = new PerformerEntity
            {
                Id = 1,
                Name = performerName,
                StartYear = startYear,
                PerformerType = performerType,
                Genre = genre
            };

            var performer2 = new PerformerEntity
            {
                Id = 2,
                Name = performerName,
                StartYear = startYear,
                PerformerType = performerType,
                Genre = genre
            };

            var repository = new PerformerRepository(dbContext);
            repository.Add(performer1);
            repository.Add(performer2);
        }


        [TestMethod]
        public void ClearRepositoryShouldReturnCountCero()
        {
            string performerName = "Adele";
            string startYear = "2006";
            PerformerType performerType = PerformerType.SOLO_ARTIST;
            GenreEntity genre = new GenreEntity()
            {
                Id = 1,
                GenreName = "Pop-Soul"
            };

            var performer = new PerformerEntity
            {
                Id = 1,
                Name = performerName,
                StartYear = startYear,
                PerformerType = performerType,
                Genre = genre
            };

            var repository = new PerformerRepository(dbContext);
            repository.Add(performer);
            repository.Clear();

            Assert.IsTrue(repository.GetAll().ToList().Count == 0);
        }

        [TestMethod]
        public void ShouldRepositoryBeEmptyWhenPerformerDeleted()
        {
            string performerName = "Adele";
            string startYear = "2006";
            PerformerType performerType = PerformerType.SOLO_ARTIST;
            GenreEntity genre = new GenreEntity()
            {
                Id = 1,
                GenreName = "Pop-Soul"
            };

            var performer = new PerformerEntity
            {
                Id = 1,
                Name = performerName,
                StartYear = startYear,
                PerformerType = performerType,
                Genre = genre
            };

            var repository = new PerformerRepository(dbContext);
            repository.Add(performer);
            repository.Delete(performer.Id);

            Assert.IsTrue(repository.IsEmpty());
        }

        [TestMethod]
        public void ExistPerformerByIdReturnsTrue()
        {
            string performerName = "Los Piojos";
            string startYear = "1988";
            PerformerType performerType = PerformerType.BAND;
            GenreEntity genre = new GenreEntity()
            {
                Id = 1,
                GenreName = "Rock"
            };
            List<string> artists = new List<string>();
            artists.Add("Andrés Ciro Martínez");
            artists.Add("Miguel Ángel Rodríguez");
            artists.Add("Sebastián Cardero");
            artists.Add("Juan Ignacio Bisio");

            var performer = new PerformerEntity
            {
                Id = 1,
                Name = performerName,
                StartYear = startYear,
                PerformerType = performerType,
                Genre = genre,
                Artists = artists
            };

            var repository = new PerformerRepository(dbContext);
            repository.Add(performer);

            Assert.IsTrue(repository.Exists(performer.Id));
        }

        [TestMethod]
        public void ExistPerformerByNameReturnsTrue()
        {
            string performerName = "Bryan Adams";
            string startYear = "1975";
            PerformerType performerType = PerformerType.SOLO_ARTIST;
            GenreEntity genre = new GenreEntity()
            {
                Id = 1,
                GenreName = "Rock"
            };

            var performer = new PerformerEntity
            {
                Id = 1,
                Name = performerName,
                StartYear = startYear,
                PerformerType = performerType,
                Genre = genre
            };

            var repository = new PerformerRepository(dbContext);
            repository.Add(performer);

            Assert.IsTrue(repository.ExistPerformerName(performer.Name));
        }

        [TestMethod]
        public void DeletedPerformerShouldNotExist()
        {
            string performerName = "Bryan Adams";
            string startYear = "1975";
            PerformerType performerType = PerformerType.SOLO_ARTIST;
            GenreEntity genre = new GenreEntity()
            {
                Id = 1,
                GenreName = "Rock"
            };

            var performer = new PerformerEntity
            {
                Id = 1,
                Name = performerName,
                StartYear = startYear,
                PerformerType = performerType,
                Genre = genre
            };

            var repository = new PerformerRepository(dbContext);
            repository.Add(performer);
            repository.Delete(performer.Id);

            Assert.IsFalse(repository.Exists(performer.Id));
        }

        [TestMethod]
        [ExpectedException(typeof(RepositoryException))]
        public void DeleteNonExistentPerformerThrowsException()
        {
            string performerName = "Enanitos Verdes";
            string startYear = "1979";
            PerformerType performerType = PerformerType.BAND;
            GenreEntity genre = new GenreEntity()
            {
                Id = 1,
                GenreName = "Rock"
            };
            List<string> artists = new List<string>();
            artists.Add("Marciano Cantero");
            artists.Add("Felipe Staiti");
            artists.Add("Jota Morelli");
            artists.Add("Juan Pablo Staiti");

            var performer = new PerformerEntity
            {
                Id = 1,
                Name = performerName,
                StartYear = startYear,
                PerformerType = performerType,
                Genre = genre,
                Artists = artists
            };

            var repository = new PerformerRepository(dbContext);
            repository.Delete(performer.Id);
        }

        [TestMethod]
        public void SearchPerformerByNameTestCorrect()
        {
            string performerName = "Keane";
            string startYear = "1995";
            PerformerType performerType = PerformerType.BAND;
            GenreEntity genre = new GenreEntity()
            {
                Id = 1,
                GenreName = "Alternative Rock"
            };
            List<string> artists = new List<string>();
            artists.Add("Tom Chaplin");
            artists.Add("Tim Rice-Oxley");
            artists.Add("Richard Hughes");
            artists.Add("Jesse Quin");

            var performer = new PerformerEntity
            {
                Id = 1,
                Name = performerName,
                StartYear = startYear,
                PerformerType = performerType,
                Genre = genre,
                Artists = artists
            };

            var repository = new PerformerRepository(dbContext);
            repository.Add(performer);

            PerformerEntity found = repository.Get(g => g.Name.Equals(performer.Name));

            Assert.IsNotNull(found);
            Assert.AreEqual(performer.Name, found.Name);
        }

        [TestMethod]
        public void ShouldCreatedDateBeTheSameDay()
        {
            string performerName = "Bob Dylan";
            string startYear = "1959";
            PerformerType performerType = PerformerType.SOLO_ARTIST;
            GenreEntity genre = new GenreEntity()
            {
                Id = 1,
                GenreName = "Folk rock"
            };

            string performerName2 = "Rock and roll";
            string startYear2 = "1959";
            PerformerType performerType2 = PerformerType.SOLO_ARTIST;
            GenreEntity genre2 = new GenreEntity()
            {
                Id = 1,
                GenreName = "Folk rock"
            };


            var performer1 = new PerformerEntity
            {
                Id = 1,
                Name = performerName,
                StartYear = startYear,
                PerformerType = performerType,
                Genre = genre
            };

            var performer2 = new PerformerEntity
            {
                Id = 1,
                Name = performerName2,
                StartYear = startYear2,
                PerformerType = performerType2,
                Genre = genre2
            };

            var repository = new PerformerRepository(dbContext);
            repository.Add(performer1);
            repository.Add(performer2);
            Assert.IsTrue(
                performer1.CreatedAt.Day.Equals(performer2.CreatedAt.Day)
                && performer1.CreatedAt.Year.Equals(performer2.CreatedAt.Year)
                && performer1.CreatedAt.Month.Equals(performer2.CreatedAt.Month));
        }

        [TestMethod]
        public void UpdateEntityValuesSuccessfully()
        {
            string performerName = "Ann Lennox";
            string startYear = "1976";
            PerformerType performerType = PerformerType.SOLO_ARTIST;
            GenreEntity genre = new GenreEntity()
            {
                Id = 1,
                GenreName = "Synth Pop"
            };

            var performer = new PerformerEntity
            {
                Id = 1,
                Name = performerName,
                StartYear = startYear,
                PerformerType = performerType,
                Genre = genre
            };

            var repository = new PerformerRepository(dbContext);
            repository.Add(performer);

            string newName = "Annie Lennox";

            performer.Name = newName;

            repository.Update(performer);

            var updatedPerformer = repository.Get(performer.Id);

            Assert.IsTrue(updatedPerformer.Name.Equals(newName));
        }

        [TestMethod]
        public void UpdateEntityValuesNullThenShouldMantainPreviousValues()
        {
            string performerName = "Attaque 77";
            string startYear = "1987";
            PerformerType performerType = PerformerType.BAND;
            GenreEntity genre = new GenreEntity()
            {
                Id = 1,
                GenreName = "Punk rock"
            };
            List<string> artists = new List<string>();
            artists.Add("Mariano Gabriel Martínez");
            artists.Add("Luciano Scaglione");
            artists.Add("Leonardo De Cecco");
            artists.Add("Martín Locarnini");

            var performer = new PerformerEntity
            {
                Id = 1,
                Name = performerName,
                StartYear = startYear,
                PerformerType = performerType,
                Genre = genre,
                Artists = artists
            };

            var repository = new PerformerRepository(dbContext);
            repository.Add(performer);

            repository.Update(new PerformerEntity { Id = performer.Id, Name = null });

            var updatedPerformer = repository.Get(performer.Id);


            Assert.IsTrue(updatedPerformer.Name.Equals(performer.Name));
        }
    }
}
