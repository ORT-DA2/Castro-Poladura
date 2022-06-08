using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;
using TicketPal.DataAccess.Repository;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Exceptions;

namespace TicketPal.DataAccess.Tests.Respository
{
    [TestClass]
    public class GenreRepositoryTests : RepositoryBaseConfigTests
    {
        [TestMethod]
        public async Task SaveGenreSuccessfully()
        {
            string genreName = "Country";

            var genre = new GenreEntity
            {
                Id = 1,
                Name = genreName
            };


            var repository = new GenreRepository(dbContext);
            await repository.Add(genre);

            Assert.IsTrue((await repository.GetAll()).ToList().FirstOrDefault().Name == genreName);
        }

        [TestMethod]
        [ExpectedException(typeof(RepositoryException))]
        public async Task SaveDuplicateGenresShouldThrowException()
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
            await repository.Add(genre1);
            await repository.Add(genre2);
        }


        [TestMethod]
        public async Task ClearRepositoryShouldReturnCountCero()
        {
            var genre = new GenreEntity
            {
                Id = 1,
                Name = "Classic"
            };

            var repository = new GenreRepository(dbContext);
            await repository.Add(genre);
            repository.Clear();

            Assert.IsTrue((await repository.GetAll()).Count == 0);
        }

        [TestMethod]
        public async Task ShouldRepositoryBeEmptyWhenGenreDeleted()
        {
            var genre = new GenreEntity
            {
                Id = 1,
                Name = "Disco"
            };

            var repository = new GenreRepository(dbContext);
            await repository.Add(genre);
            await repository.Delete(genre.Id);

            Assert.IsTrue(repository.IsEmpty());
        }

        [TestMethod]
        public async Task ExistGenreByIdReturnsTrue()
        {
            var genre = new GenreEntity
            {
                Id = 1,
                Name = "Funk"
            };

            var repository = new GenreRepository(dbContext);
            await repository.Add(genre);

            Assert.IsTrue(repository.Exists(genre.Id));
        }

        [TestMethod]
        public async Task DeletedGenreShouldNotExist()
        {
            var genre = new GenreEntity
            {
                Id = 1,
                Name = "Metal"
            };

            var repository = new GenreRepository(dbContext);
            await repository.Add(genre);
            await repository.Delete(genre.Id);

            Assert.IsFalse(repository.Exists(genre.Id));
        }

        [TestMethod]
        [ExpectedException(typeof(RepositoryException))]
        public async Task DeleteNonExistentGenreThrowsException()
        {
            var genre = new GenreEntity
            {
                Id = 1,
                Name = "Folk"
            };

            var repository = new GenreRepository(dbContext);
            await repository.Delete(genre.Id);
        }

        [TestMethod]
        public async Task SearchGenreByNameTestCorrect()
        {
            var genre = new GenreEntity
            {
                Id = 1,
                Name = "Jazz"
            };

            var repository = new GenreRepository(dbContext);
            await repository.Add(genre);

            GenreEntity found = await repository.Get(g => g.Name.Equals(genre.Name));

            Assert.IsNotNull(found);
            Assert.AreEqual(genre.Name, found.Name);
        }

        [TestMethod]
        public async Task ShouldCreatedDateBeTheSameDay()
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
            await repository.Add(genre1);
            await repository.Add(genre2);
            Assert.IsTrue(
                genre1.CreatedAt.Day.Equals(genre2.CreatedAt.Day)
                && genre1.CreatedAt.Year.Equals(genre2.CreatedAt.Year)
                && genre1.CreatedAt.Month.Equals(genre2.CreatedAt.Month));
        }

        [TestMethod]
        public async Task UpdateEntityValuesSuccessfully()
        {
            var genre = new GenreEntity
            {
                Id = 1,
                Name = "Reggae"
            };

            var repository = new GenreRepository(dbContext);
            await repository.Add(genre);

            var newName = "Punk";

            genre.Name = newName;

            repository.Update(genre);

            var updatedGenre = await repository.Get(genre.Id);

            Assert.IsTrue(updatedGenre.Name.Equals(newName));
        }

        [TestMethod]
        public async Task UpdateEntityValuesNullThenShouldMantainPreviousValues()
        {
            var genre = new GenreEntity
            {
                Id = 1,
                Name = "Rock"
            };

            var repository = new GenreRepository(dbContext);
            await repository.Add(genre);

            repository.Update(new GenreEntity { Id = genre.Id, Name = null });

            var updatedGenre = await repository.Get(genre.Id);


            Assert.IsTrue(updatedGenre.Name.Equals(genre.Name));
        }
    }
}
