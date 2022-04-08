using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TicketPal.DataAccess.Tests
{
    [TestClass]
    public class GenreEntityTests
    {
        private GenreEntity genre;
        private int idGenre;
        private string genreName;

        [TestInitialize]
        public void Initialize()
        {
            idGenre = 1;
            genreName = "Blues";
            genre = new GenreEntity();
            genre.IdGenre = idGenre;
            genre.GenreName = genreName;
        }

        [TestMethod]
        public void SetGenreTest()
        {
            int idGenre = 2;
            string genreName = "Rock";
            genre.IdGenre = idGenre;
            genre.GenreName = genreName;

            Assert.AreEqual(genre.IdGenre, idGenre);
            Assert.AreEqual(genre.GenreName, genreName);
        }

        [TestMethod]
        public void GetGenreTest()
        {
            int idGenre = genre.IdGenre;
            string genreName = genre.GenreName;

            Assert.AreEqual(genre.IdGenre, idGenre);
            Assert.AreEqual(genre.GenreName, genreName);
        }
    }
}
