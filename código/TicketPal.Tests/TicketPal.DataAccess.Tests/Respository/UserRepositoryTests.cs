using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicketPal.DataAccess.Repository;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Exceptions;

namespace TicketPal.DataAccess.Tests.Respository
{
    [TestClass]
    public class UserRepositoryTests : RepositoryBaseConfigTests
    {
        [TestMethod]
        public void SaveTwoUserToDataBaseShouldReturnCountAsTwo()
        {
            var user1 = new UserEntity
            {
                Id = 1,
                Firstname = "SomeName1",
                Lastname = "SomeSurname1",
                Email = "user1@example.com",
                Password = "myPassword"
            };
            var user2 = new UserEntity
            {
                Id = 2,
                Firstname = "SomeName2",
                Lastname = "SomeSurname2",
                Email = "user2@example.com",
                Password = "myPassword"
            };

            var repository = new UserRepository(dbContext);
            repository.Add(user1);
            repository.Add(user2);

            Assert.IsTrue(repository.GetAll().ToList().Count == 2);

        }

        [TestMethod]
        [ExpectedException(typeof(RepositoryException))]
        public void SaveUserWithDuplicateEmailShouldThrowException()
        {
            string email = "same@example.com";

            var user1 = new UserEntity
            {
                Id = 1,
                Firstname = "SomeName1",
                Lastname = "SomeSurname1",
                Email = email,
                Password = "myPassword"
            };
            var user2 = new UserEntity
            {
                Id = 2,
                Firstname = "SomeName2",
                Lastname = "SomeSurname2",
                Email = email,
                Password = "myPassword"
            };

            var repository = new UserRepository(dbContext);
            repository.Add(user1);
            repository.Add(user2);


        }


        [TestMethod]
        public void ClearRepositoryShouldReturnCountCero()
        {
            var user = new UserEntity
            {
                Id = 1,
                Firstname = "SomeName1",
                Lastname = "SomeSurname1",
                Email = "user@example.com",
                Password = "myPassword"
            };

            var repository = new UserRepository(dbContext);
            repository.Add(user);
            repository.Clear();

            Assert.IsTrue(repository.GetAll().ToList().Count == 0);


        }

        [TestMethod]
        public void ShouldRepositoryBeEmptyWhenUserDeleted()
        {
            var user = new UserEntity
            {
                Id = 1,
                Firstname = "SomeName1",
                Lastname = "SomeSurname1",
                Email = "user@example.com",
                Password = "myPassword",
                Role = "admin"
            };

            var repository = new UserRepository(dbContext);
            repository.Add(user);
            repository.Delete(user.Id);

            Assert.IsTrue(repository.IsEmpty());


        }

        [TestMethod]
        public void SaveUserAndShouldExistReturnTrue()
        {
            var user = new UserEntity
            {
                Id = 1,
                Firstname = "SomeName1",
                Lastname = "SomeSurname1",
                Email = "user@example.com",
                Password = "myPassword"
            };

            var repository = new UserRepository(dbContext);
            repository.Add(user);

            Assert.IsTrue(repository.Exists(user.Id));


        }

        [TestMethod]
        public void ShouldGetSavedUserAndCompareEmail()
        {
            var user = new UserEntity
            {
                Id = 1,
                Firstname = "SomeName1",
                Lastname = "SomeSurname1",
                Email = "user@example.com",
                Password = "myPassword"
            };

            var repository = new UserRepository(dbContext);
            repository.Add(user);

            Assert.IsTrue(repository.Get(user.Id).Email.Equals(user.Email));


        }

        [TestMethod]
        public void AddUserAndDeleteShouldNotExist()
        {
            var user = new UserEntity
            {
                Id = 1,
                Firstname = "SomeName1",
                Lastname = "SomeSurname1",
                Email = "user@example.com",
                Password = "myPassword",
                Role = "admin"
            };

            var repository = new UserRepository(dbContext);
            repository.Add(user);
            repository.Delete(user.Id);

            Assert.IsFalse(repository.Exists(user.Id));
        }

        [TestMethod]
        [ExpectedException(typeof(RepositoryException))]
        public void DeleteNonExistentUserThrowsException()
        {
            var user = new UserEntity
            {
                Id = 1,
                Firstname = "SomeName1",
                Lastname = "SomeSurname1",
                Email = "user@example.com",
                Password = "myPassword"
            };

            var repository = new UserRepository(dbContext);
            repository.Delete(user.Id);
        }

        [TestMethod]
        public void SearchUserByEmailTestCorrect()
        {
            var user = new UserEntity
            {
                Id = 1,
                Firstname = "SomeName1",
                Lastname = "SomeSurname1",
                Email = "user1@example.com",
                Password = "myPassword"
            };

            var repository = new UserRepository(dbContext);
            repository.Add(user);

            UserEntity found = repository.Get(u => u.Email.Equals(user.Email));

            Assert.IsNotNull(found);
            Assert.AreEqual(user.Email, found.Email);
        }

        [TestMethod]
        public void SearchUserByRoleCountShouldBeTwo()
        {
            var role = "admin";
            var user1 = new UserEntity
            {
                Id = 1,
                Role = role,
                Email = "user1@example.com",
                Password = "myPassword"
            };
            var user2 = new UserEntity
            {
                Id = 2,
                Role = role,
                Email = "user2@example.com",
                Password = "myPassword"
            };
            var user3 = new UserEntity
            {
                Id = 3,
                Role = "pacient",
                Email = "user3@example.com",
                Password = "myPassword"
            };

            var repository = new UserRepository(dbContext);
            repository.Add(user1);
            repository.Add(user2);
            repository.Add(user3);

            IEnumerable<UserEntity> found = repository.GetAll(u => u.Role.Equals(role));

            List<UserEntity> resultToList = found.ToList();
            Assert.IsTrue(resultToList.Count == 2);
        }

        [TestMethod]
        public void ShouldCreatedDateBeTheSameDay()
        {

            var user1 = new UserEntity
            {
                Id = 1,
                Email = "user1@example.com",
                Password = "myPassword"
            };
            var user2 = new UserEntity
            {
                Id = 2,
                Email = "user2@example.com",
                Password = "myPassword"
            };

            var repository = new UserRepository(dbContext);
            repository.Add(user1);
            repository.Add(user2);
            Assert.IsTrue(
                user1.CreatedAt.Day.Equals(user2.CreatedAt.Day)
                && user1.CreatedAt.Year.Equals(user2.CreatedAt.Year)
                && user1.CreatedAt.Month.Equals(user2.CreatedAt.Month));

        }

        [TestMethod]
        public void ShouldUpdateEntityValuesBeUpdated()
        {


            var user = new UserEntity
            {
                Id = 1,
                Firstname = "SomeName1",
                Lastname = "SomeSurname1",
                Email = "user@example.com",
                Password = "myPassword",
                Role = "admin"
            };


            var repository = new UserRepository(dbContext);
            repository.Add(user);

            var newName = "newName";
            var newSurname = "newSurname";
            var newEmail = "myNewEmail@example.com";
            var newPassword = "myNewPassword";

            user.Firstname = newName;
            user.Lastname = newSurname;
            user.Email = newEmail;
            user.Password = newPassword;

            repository.Update(user);

            var updatedUser = repository.Get(user.Id);

            Assert.IsTrue(
                updatedUser.Email.Equals(newEmail)
                && updatedUser.Password.Equals(newPassword)
                && updatedUser.Lastname.Equals(newSurname)
                && updatedUser.Firstname.Equals(newName)
                );


        }

        [TestMethod]
        public void ShouldUpdateEntityValuesNullThenShouldMantainPreviousValues()
        {


            var user = new UserEntity
            {
                Id = 1,
                Firstname = "SomeName1",
                Lastname = "SomeSurname1",
                Email = "user@example.com",
                Password = "myPassword",
                Role = "admin"
            };


            var repository = new UserRepository(dbContext);
            repository.Add(user);


            repository.Update(new UserEntity { Id = user.Id, Firstname = null, Lastname = null, Email = null, Password = null, Role = null });

            var updatedUser = repository.Get(user.Id);


            Assert.IsTrue(
                updatedUser.Email.Equals(user.Email)
                && updatedUser.Password.Equals(user.Password)
                && updatedUser.Lastname.Equals(user.Lastname)
                && updatedUser.Firstname.Equals(user.Firstname)
                );


        }
    }
}