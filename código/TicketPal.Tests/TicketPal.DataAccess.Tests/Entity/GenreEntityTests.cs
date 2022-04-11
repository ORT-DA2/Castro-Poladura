using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicketPal.Domain.Entity;

namespace TicketPal.DataAccess.Tests.Entity
{
    [TestClass]
    public class GenreEntityTests
    {
        private GenreEntity genre;
        private int idGenre;

        private DateTime createdAt;
        private DateTime updatedAt;

        [TestInitialize]
        public void Initialize()
        {
            createdAt = new DateTime(2022, 04, 06);
            updatedAt = new DateTime(2022, 04, 07);
            genre = new GenreEntity();
            idGenre = 1;

        }

        [TestMethod]
        public void GetGenreTest()
        {
            string genreName = "Rock";
            genre.GenreName = genreName;
            genre.Id = idGenre;
            genre.CreatedAt = createdAt;
            genre.UpdatedAt = updatedAt;

            Assert.AreEqual(genre.Id, idGenre);
            Assert.AreEqual(genre.GenreName, genreName);
        }

    }
}
