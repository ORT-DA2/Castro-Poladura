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
                Name = genreName
            };


            var repository = new GenreRepository(dbContext);
            repository.Add(genre);

            Assert.IsTrue(repository.GetAll().ToList().FirstOrDefault().Name == genreName);
        }

        [TestMethod]
        [ExpectedException(typeof(RepositoryException))]
        public void SaveDuplicateGenresShouldThrowException()
        {
            string genreName = "Pop";

            var genre1 = new GenreEntity
            {
                Id = 1,
                Name = genreName
            };
            var genre2 = new GenreEntity
            {
                Id = 1,
                Name = genreName
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
                Name = "Classic"
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
                Name = "Disco"
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
                Name = "Funk"
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
                Name = "Metal"
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
                Name = "Folk"
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
                Name = "Jazz"
            };

            var repository = new GenreRepository(dbContext);
            repository.Add(genre);

            GenreEntity found = repository.Get(g => g.Name.Equals(genre.Name));

            Assert.IsNotNull(found);
            Assert.AreEqual(genre.Name, found.Name);
        }

        [TestMethod]
        public void ShouldCreatedDateBeTheSameDay()
        {
            var genre1 = new GenreEntity
            {
                Id = 1,
                Name = "Salsa"
            };
            var genre2 = new GenreEntity
            {
                Id = 2,
                Name = "Blues"
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
                Name = "Reggae"
            };

            var repository = new GenreRepository(dbContext);
            repository.Add(genre);

            var newName = "Punk";

            genre.Name = newName;

            repository.Update(genre);

            var updatedGenre = repository.Get(genre.Id);

            Assert.IsTrue(updatedGenre.Name.Equals(newName));
        }

        [TestMethod]
        public void UpdateEntityValuesNullThenShouldMantainPreviousValues()
        {
            var genre = new GenreEntity
            {
                Id = 1,
                Name = "Rock"
            };

            var repository = new GenreRepository(dbContext);
            repository.Add(genre);

            repository.Update(new GenreEntity { Id = genre.Id, Name = null });

            var updatedGenre = repository.Get(genre.Id);


            Assert.IsTrue(updatedGenre.Name.Equals(genre.Name));
        }
    }
}
