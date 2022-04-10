using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicketPal.DataAccess.Entity;

namespace TicketPal.DataAccess.Tests.Entity
{
    [TestClass]
    public class GenreEntityTests
    {
        private GenreEntity genre;
        private int idGenre;
        private string genreName;
        private DateTime createdAt;
        private DateTime updatedAt;

        [TestInitialize]
        public void Initialize()
        {
            genreName = "Blues";
            createdAt = new DateTime(2022, 04, 06);
            updatedAt = new DateTime(2022, 04, 07);
            genre = new GenreEntity();
            genre.Id = idGenre;
            genre.GenreName = genreName;
            genre.CreatedAt = createdAt;
            genre.UpdatedAt = updatedAt;

        }

        [TestMethod]
        public void SetGenreTest()
        {
            string genreName = "Rock";
            genre.GenreName = genreName;
            genre.Id = idGenre;
            genre.CreatedAt = createdAt;
            genre.UpdatedAt = updatedAt;

            Assert.AreEqual(genre.Id, idGenre);
            Assert.AreEqual(genre.GenreName, genreName);
        }

        [TestMethod]
        public void GetGenreTest()
        {
            int idGenre = genre.Id;
            string genreName = genre.GenreName;

            Assert.AreEqual(genre.Id, idGenre);
            Assert.AreEqual(genre.GenreName, genreName);
        }
    }
}
