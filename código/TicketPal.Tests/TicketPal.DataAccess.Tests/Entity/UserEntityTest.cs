using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicketPal.Domain.Entity;

namespace TicketPal.DataAccess.Tests.Entity
{
    [TestClass]
    public class UserEntityTest
    {
        [TestMethod]
        public void CreateNewUserNotNullTest()
        {
            UserEntity user = new UserEntity();
            Assert.IsNotNull(user);
        }
        [TestMethod]
        public void UserNotInitializedIsNull()
        {
            UserEntity user = null;
            Assert.IsNull(user);
        }

        [TestMethod]
        public void SetUserIdEquals()
        {
            int id = 1;
            UserEntity user = new UserEntity
            {
                Id = id
            };
            Assert.AreEqual(id, user.Id);
        }

        [TestMethod]
        public void NameNotNull()
        {
            string name = "someName";

            UserEntity request = new UserEntity
            {
                FirstName = name
            };

            Assert.AreEqual(request.FirstName, name);
        }

        [TestMethod]
        public void SurnameNotNull()
        {
            string surname = "someSurname";

            UserEntity request = new UserEntity
            {
                LastName = surname
            };

            Assert.AreEqual(request.LastName, surname);
        }

        [TestMethod]
        public void PasswordNotNull()
        {
            string password = "password";

            UserEntity request = new UserEntity
            {
                Password = password
            };

            Assert.AreEqual(request.Password, password);
        }

        [TestMethod]
        public void SetUserEmailEquals()
        {
            string email = "someEmail@example.com";
            UserEntity user = new UserEntity
            {
                Email = email
            };
            Assert.AreEqual(email, user.Email);
        }
        [TestMethod]
        public void SetUserPasswordEquals()
        {
            string password = "somePassword";
            UserEntity user = new UserEntity
            {
                Password = password
            };
            Assert.AreEqual(password, user.Password);
        }


        [TestMethod]
        public void SetUserCreatedAtEquals()
        {
            DateTime actualDate = DateTime.Now;
            UserEntity user = new UserEntity
            {
                CreatedAt = actualDate
            };
            Assert.AreEqual(actualDate, user.CreatedAt);
        }

        [TestMethod]
        public void UserCreatedAtGreaterThanTodaysDate()
        {
            DateTime actualDate = DateTime.Now;
            UserEntity user = new UserEntity
            {
                CreatedAt = new DateTime(1984, 03, 16)
            };
            Assert.IsTrue(actualDate > user.CreatedAt);
        }

        [TestMethod]
        public void UserCreatedAtLessThanTodaysDate()
        {
            DateTime createdDate = new DateTime(1984, 03, 16);
            UserEntity user = new UserEntity
            {
                CreatedAt = createdDate
            };
            Assert.IsTrue(DateTime.Now > user.CreatedAt);
        }

        [TestMethod]
        public void SetUserUpdatedAtEquals()
        {
            DateTime updatedDate = DateTime.Now;
            UserEntity user = new UserEntity
            {
                UpdatedAt = updatedDate
            };
            Assert.AreEqual(updatedDate, user.UpdatedAt);
        }

        [TestMethod]
        public void UserUpdatedAtGreaterThanTodaysDate()
        {
            DateTime updatedDate = DateTime.Now;
            UserEntity user = new UserEntity
            {
                UpdatedAt = new DateTime(1984, 03, 16)
            };
            Assert.IsTrue(updatedDate > user.UpdatedAt);
        }

        [TestMethod]
        public void UserUpdatedAtLessThanTodaysDate()
        {
            DateTime updatedDate = new DateTime(1984, 03, 16);
            UserEntity user = new UserEntity
            {
                UpdatedAt = updatedDate
            };
            Assert.IsTrue(DateTime.Now > user.UpdatedAt);
        }

        [TestMethod]
        public void UserRoleEquals()
        {
            string role = "admin";

            UserEntity user = new UserEntity
            {
                Role = role
            };
            Assert.IsTrue(user.Role.Equals(role));
        }
    }
}