using BC = BCrypt.Net.BCrypt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Exceptions;

namespace TicketPal.BusinessLogic.Tests.Services.Users
{
    [TestClass]
    public class UserServiceTest : BaseServiceTest
    {
        private string jwtTestSecret;
        private string userPassword;
        private IUserService userService;

        [TestMethod]
        public void UserAuthenticateCorrectly()
        {
            int id = 1;

            var authRequest = new AuthenticationRequest
            {
                Email = "someone@example.com",
                Password = userPassword
            };

            var dbUser = new UserEntity
            {
                Id = id,
                FirstName = "John",
                LastName = "Doe",
                Email = "someone@example.com",
                Password = BC.HashPassword(userPassword),
                Role = UserRoles.ADMIN
            };

            this.usersMock.Setup(r => r.Get(It.IsAny<Expression<Func<UserEntity, bool>>>()))
                .Returns(dbUser);

            User authenticatedUser = userService.Authenticate(authRequest);

            Assert.IsNotNull(authenticatedUser);
            Assert.IsNotNull(authenticatedUser.Token);
            Assert.IsTrue(authenticatedUser.Id == dbUser.Id);
        }

        [TestMethod]
        public void UserAuthenticatePasswordIncorrect()
        {
            int id = 1;

            var authRequest = new AuthenticationRequest
            {
                Email = "someone@example.com",
                Password = "aDifferentWrongPassword"
            };

            var dbUser = new UserEntity
            {
                Id = id,
                FirstName = "John",
                LastName = "Doe",
                Email = "someone@example.com",
                Password = BC.HashPassword(userPassword),
                Role = UserRoles.ADMIN
            };

            this.usersMock.Setup(r => r.Get(It.IsAny<Expression<Func<UserEntity, bool>>>()))
                    .Returns(dbUser);

            User authenticatedUser = userService.Authenticate(authRequest);

            Assert.IsNull(authenticatedUser);
        }

        [TestMethod]
        public void UserAuthenticateNoUserFound()
        {
            int id = 1;

            var authRequest = new AuthenticationRequest
            {
                Email = "someone@example.com",
                Password = userPassword
            };

            var dbUser = new UserEntity
            {
                Id = id,
                FirstName = "John",
                LastName = "Doe",
                Email = "someone@example.com",
                Password = BC.HashPassword(userPassword),
                Role = UserRoles.ADMIN
            };


            this.usersMock.Setup(r => r.Get(It.IsAny<Expression<Func<UserEntity, bool>>>()))
                    .Returns(It.IsAny<UserEntity>);

            User authenticatedUser = userService.Authenticate(authRequest);

            Assert.IsNull(authenticatedUser);

        }

        [TestMethod]
        public void UserRegistrationCorrect()
        {
            var signInRequest = new SignInRequest
            {
                Firstname = "John",
                Lastname = "Doe",
                Email = "someone@example.com",
                Password = "myTestEnteredPassword",
                Role = UserRoles.SPECTATOR
            };

            this.usersMock.Setup(r => r.Exists(It.IsAny<int>())).Returns(false);
            this.usersMock.Setup(r => r.Add(It.IsAny<UserEntity>())).Verifiable();

            OperationResult result = userService.Register(signInRequest);

            Assert.IsTrue(result.ResultCode == ResultCode.SUCCESS);

        }

        [TestMethod]
        public void UserRegistrationThrowsExceptionFail()
        {
            var signInRequest = new SignInRequest
            {
                Firstname = "John",
                Lastname = "Doe",
                Email = "someone@example.com",
                Password = "myTestEnteredPassword",
                Role = UserRoles.SPECTATOR
            };

            this.usersMock.Setup(r => r.Exists(It.IsAny<int>())).Returns(false);
            this.usersMock.Setup(r => r.Add(It.IsAny<UserEntity>()))
                .Throws(new RepositoryException());

            OperationResult result = userService.Register(signInRequest);

            Assert.IsTrue(result.ResultCode == ResultCode.FAIL);

        }


        [TestMethod]
        public void ShouldGetAllUsers()
        {
            IEnumerable<UserEntity> dbAccounts = new List<UserEntity>()
            {
                new UserEntity{Id=1,FirstName="Jennifer",LastName="Garner",Email="user1@example.com",Role=UserRoles.ADMIN},
                new UserEntity{Id=2,FirstName="John",LastName="Doe",Email="user2@example.com",Role=UserRoles.SELLER},
                new UserEntity{Id=3,FirstName="Jane",LastName="Doe",Email="user3@example.com",Role=UserRoles.SUPERVISOR},
                new UserEntity{Id=3,FirstName="Steve",LastName="Black",Email="user4@example.com",Role=UserRoles.SPECTATOR}
            };

            this.usersMock.Setup(r => r.GetAll()).Returns(dbAccounts);

            IEnumerable<User> result = userService.GetUsers();

            Assert.IsTrue(result.ToList().Count == 4);

        }

        [TestMethod]
        public void GetUserById()
        {
            int id = 1;

            var dbUser = new UserEntity
            {
                Id = id,
                FirstName = "John",
                LastName = "Doe",
                Email = "someone@example.com",
                Password = BC.HashPassword(userPassword),
                Role = UserRoles.ADMIN
            };

            this.usersMock.Setup(r => r.Get(It.IsAny<int>())).Returns(dbUser);

            User account = userService.GetUser(id);

            Assert.IsNotNull(account);
            Assert.IsTrue(id == account.Id);
        }

        [TestMethod]
        public void DeleteAccountCorrectly()
        {
            var id = 1;

            var dbUser = new UserEntity
            {
                Id = id,
                FirstName = "John",
                LastName = "Doe",
                Email = "someone@example.com",
                Password = BC.HashPassword(userPassword),
                Role = UserRoles.ADMIN
            };

            this.usersMock.Setup(r => r.Get(It.IsAny<int>())).Returns(dbUser);

            OperationResult result = userService.DeleteAccountById(id);

            Assert.IsTrue(result.ResultCode == ResultCode.SUCCESS);
        }

        [TestMethod]
        public void UpdateUserCorrectly()
        {

            var updateRequest = new UpdateSpectatorUserRequest
            {
                FirstName = "David",
                Lastname = "Foster",
                Password = "somePassword"
            };

            this.usersMock.Setup(p => p.Update(It.IsAny<UserEntity>())).Verifiable();

            OperationResult expected = userService.UpdateAccount(updateRequest);

            Assert.IsTrue(expected.ResultCode == ResultCode.SUCCESS);
        }

        [TestMethod]
        public void UpdateUserThrowsError()
        {

            var updateRequest = new UpdateSpectatorUserRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Password = BC.HashPassword(userPassword)
            };

            this.usersMock.Setup(p => p.Update(It.IsAny<UserEntity>())).Throws(new RepositoryException());

            OperationResult expected = userService.UpdateAccount(updateRequest);

            Assert.IsTrue(expected.ResultCode == ResultCode.FAIL);
        }

        [TestMethod]
        public void UpdateUserRoleNotAllowed()
        {

            var updateRequest = new UpdateUserRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "someone@example.com",
                Password = BC.HashPassword(userPassword),
                Role = "nonExistentRole"
            };

            OperationResult expected = userService.UpdateAccount(updateRequest);

            Assert.IsTrue(expected.ResultCode == ResultCode.FAIL);
        }

        [TestMethod]
        public void DeleteAccountThatDoesntExist()
        {
            var id = 1;

            this.usersMock.Setup(r => r.Delete(It.IsAny<int>())).Throws(new RepositoryException());

            OperationResult result = userService.DeleteAccountById(id);

            Assert.IsTrue(result.ResultCode == ResultCode.FAIL);
        }

        [TestMethod]
        public void GetUserByIdIsNull()
        {
            int id = 1;

            UserEntity dbUser = null;

            this.usersMock.Setup(r => r.Get(It.IsAny<int>())).Returns(dbUser);

            User account = userService.GetUser(id);

            Assert.IsNull(account);

        }
    }
}