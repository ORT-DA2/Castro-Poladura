using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TicketPal.DataAccess.Repository;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Exceptions;

namespace TicketPal.DataAccess.Tests.Respository
{
    [TestClass]
    public class GenreRepositoryTests : RepositoryBaseConfigTests
    {
        [TestMethod]
        public void SaveGenreSuccessfully()
        {
            string genreName = "Country";

            var genre = new GenreEntity
            {
                Id = 1,
                GenreName = genreName
            };
            

            var repository = new GenreRepository(dbContext);
            repository.Add(genre);

            Assert.IsTrue(repository.GetAll().ToList().FirstOrDefault().GenreName == genreName);
        }

        [TestMethod]
        [ExpectedException(typeof(RepositoryException))]
        public void SaveDuplicateGenresShouldThrowException()
        {
            string genreName = "Pop";

            var genre1 = new GenreEntity
            {
                Id = 1,
                GenreName = genreName
            };
            var genre2 = new GenreEntity
            {
                Id = 1,
                GenreName = genreName
            };

            var repository = new GenreRepository(dbContext);
            repository.Add(genre1);
            repository.Add(genre2);
        }


        [TestMethod]
        public void ClearRepositoryShouldReturnCountCero()
        {
            var genre = new GenreEntity
            {
                Id = 1,
                GenreName = "Classic"
            };

            var repository = new GenreRepository(dbContext);
            repository.Add(genre);
            repository.Clear();

            Assert.IsTrue(repository.GetAll().ToList().Count == 0);
        }

        [TestMethod]
        public void ShouldRepositoryBeEmptyWhenGenreDeleted()
        {
            var genre = new GenreEntity
            {
                Id = 1,
                GenreName = "Disco"
            };

            var repository = new GenreRepository(dbContext);
            repository.Add(genre);
            repository.Delete(genre.Id);

            Assert.IsTrue(repository.IsEmpty());
        }

        [TestMethod]
        public void ExistGenreByIdReturnsTrue()
        {
            var genre = new GenreEntity
            {
                Id = 1,
                GenreName = "Funk"
            };

            var repository = new GenreRepository(dbContext);
            repository.Add(genre);

            Assert.IsTrue(repository.Exists(genre.Id));
        }

        [TestMethod]
        public void DeletedGenreShouldNotExist()
        {
            var genre = new GenreEntity
            {
                Id = 1,
                GenreName = "Metal"
            };

            var repository = new GenreRepository(dbContext);
            repository.Add(genre);
            repository.Delete(genre.Id);

            Assert.IsFalse(repository.Exists(genre.Id));
        }

        [TestMethod]
        [ExpectedException(typeof(RepositoryException))]
        public void DeleteNonExistentGenreThrowsException()
        {
            var genre = new GenreEntity
            {
                Id = 1,
                GenreName = "Folk"
            };

            var repository = new GenreRepository(dbContext);
            repository.Delete(genre.Id);
        }

        [TestMethod]
        public void SearchGenreByNameTestCorrect()
        {
            var genre = new GenreEntity
            {
                Id = 1,
                GenreName = "Jazz"
            };

            var repository = new GenreRepository(dbContext);
            repository.Add(genre);

            GenreEntity found = repository.Get(g => g.GenreName.Equals(genre.GenreName));

            Assert.IsNotNull(found);
            Assert.AreEqual(genre.GenreName, found.GenreName);
        }

        [TestMethod]
        public void ShouldCreatedDateBeTheSameDay()
        {
            var genre1 = new GenreEntity
            {
                Id = 1,
                GenreName = "Salsa"
            };
            var genre2 = new GenreEntity
            {
                Id = 2,
                GenreName = "Blues"
            };

            var repository = new GenreRepository(dbContext);
            repository.Add(genre1);
            repository.Add(genre2);
            Assert.IsTrue(
                genre1.CreatedAt.Day.Equals(genre2.CreatedAt.Day)
                && genre1.CreatedAt.Year.Equals(genre2.CreatedAt.Year)
                && genre1.CreatedAt.Month.Equals(genre2.CreatedAt.Month));
        }

        [TestMethod]
        public void UpdateEntityValuesSuccessfully()
        {
            var genre = new GenreEntity
            {
                Id = 1,
                GenreName = "Reggae"
            };

            var repository = new GenreRepository(dbContext);
            repository.Add(genre);

            var newName = "Punk";

            genre.GenreName = newName;

            repository.Update(genre);

            var updatedGenre = repository.Get(genre.Id);

            Assert.IsTrue(updatedGenre.GenreName.Equals(newName));
        }

        [TestMethod]
        public void UpdateEntityValuesNullThenShouldMantainPreviousValues()
        {
            var genre = new GenreEntity
            {
                Id = 1,
                GenreName = "Rock"
            };

            var repository = new GenreRepository(dbContext);
            repository.Add(genre);

            repository.Update(new GenreEntity { Id = genre.Id, GenreName = null });

            var updatedGenre = repository.Get(genre.Id);


            Assert.IsTrue(updatedGenre.GenreName.Equals(genre.GenreName));
        }
    }
}
