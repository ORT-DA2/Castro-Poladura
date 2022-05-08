using BC = BCrypt.Net.BCrypt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Exceptions;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Models.Response;
using TicketPal.Domain.Models.Request;
using TicketPal.BusinessLogic.Services.Users;
using TicketPal.Interfaces.Services.Jwt;

namespace TicketPal.BusinessLogic.Tests.Services.Users
{
    [TestClass]
    public class UserServiceTest : BaseServiceTest
    {
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
                Firstname = "John",
                Lastname = "Doe",
                Email = "someone@example.com",
                Password = BC.HashPassword(userPassword),
                Role = UserRole.ADMIN.ToString()
            };

            this.mockUserRepo.Setup(r => r.Get(It.IsAny<Expression<Func<UserEntity, bool>>>()))
                .Returns(dbUser);
            this.factoryMock.Setup(m => m.GetRepository(typeof(UserEntity)))
                .Returns(this.mockUserRepo.Object);
            this.mockJwtService.Setup(m => m.GenerateJwtToken(
                It.IsAny<string>(),
                "id",
                It.IsAny<string>()
            )).Returns("someToken");

            this.factoryMock.Setup(m => m.GetService(typeof(IJwtService)))
                .Returns(this.mockJwtService.Object);

            this.userService = new UserService(
                this.factoryMock.Object,
                this.options,
                this.mapper
            );
            User authenticatedUser = userService.Login(authRequest);

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
                Firstname = "John",
                Lastname = "Doe",
                Email = "someone@example.com",
                Password = BC.HashPassword(userPassword),
                Role = UserRole.ADMIN.ToString()
            };

            this.mockUserRepo.Setup(r => r.Get(It.IsAny<Expression<Func<UserEntity, bool>>>()))
                    .Returns(dbUser);
            this.factoryMock.Setup(m => m.GetRepository(typeof(UserEntity)))
                .Returns(this.mockUserRepo.Object);

            this.userService = new UserService(
                this.factoryMock.Object,
                this.options,
                this.mapper
            );
            User authenticatedUser = userService.Login(authRequest);

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
                Firstname = "John",
                Lastname = "Doe",
                Email = "someone@example.com",
                Password = BC.HashPassword(userPassword),
                Role = UserRole.ADMIN.ToString()
            };


            this.mockUserRepo.Setup(r => r.Get(It.IsAny<Expression<Func<UserEntity, bool>>>()))
                    .Returns(It.IsAny<UserEntity>);
            this.factoryMock.Setup(m => m.GetRepository(typeof(UserEntity)))
                .Returns(this.mockUserRepo.Object);

            this.userService = new UserService(
                this.factoryMock.Object,
                this.options,
                this.mapper
            );
            User authenticatedUser = userService.Login(authRequest);

            Assert.IsNull(authenticatedUser);
        }

        [TestMethod]
        public void UserRegistrationCorrect()
        {
            var signInRequest = new SignUpRequest
            {
                Firstname = "John",
                Lastname = "Doe",
                Email = "someone@example.com",
                Password = "myTestEnteredPassword",
                Role = UserRole.SPECTATOR.ToString()
            };

            this.mockUserRepo.Setup(r => r.Exists(It.IsAny<int>())).Returns(false);
            this.mockUserRepo.Setup(r => r.Add(It.IsAny<UserEntity>())).Verifiable();

            this.factoryMock.Setup(m => m.GetRepository(typeof(UserEntity)))
                .Returns(this.mockUserRepo.Object);

            this.userService = new UserService(
                this.factoryMock.Object,
                this.options,
                this.mapper
            );

            OperationResult result = userService.SignUp(signInRequest);

            Assert.IsTrue(result.ResultCode == ResultCode.SUCCESS);
        }

        [TestMethod]
        public void UserRegistrationThrowsExceptionFail()
        {
            var signInRequest = new SignUpRequest
            {
                Firstname = "John",
                Lastname = "Doe",
                Email = "someone@example.com",
                Password = "myTestEnteredPassword",
                Role = UserRole.SPECTATOR.ToString()
            };

            this.mockUserRepo.Setup(r => r.Exists(It.IsAny<int>())).Returns(false);
            this.mockUserRepo.Setup(r => r.Add(It.IsAny<UserEntity>()))
                .Throws(new RepositoryException());

            this.factoryMock.Setup(m => m.GetRepository(typeof(UserEntity)))
                .Returns(this.mockUserRepo.Object);

            this.userService = new UserService(
                this.factoryMock.Object,
                this.options,
                this.mapper
            );

            OperationResult result = userService.SignUp(signInRequest);

            Assert.IsTrue(result.ResultCode == ResultCode.FAIL);
        }


        [TestMethod]
        public void ShouldGetAllUsers()
        {
            IEnumerable<UserEntity> dbAccounts = new List<UserEntity>()
            {
                new UserEntity{Id=1,Firstname="Jennifer",Lastname="Garner",Email="user1@example.com",Role=UserRole.SPECTATOR.ToString()},
                new UserEntity{Id=2,Firstname="John",Lastname="Doe",Email="user2@example.com",Role=UserRole.SPECTATOR.ToString()},
                new UserEntity{Id=3,Firstname="Jane",Lastname="Doe",Email="user3@example.com",Role=UserRole.SPECTATOR.ToString()},
                new UserEntity{Id=3,Firstname="Steve",Lastname="Black",Email="user4@example.com",Role=UserRole.SPECTATOR.ToString()}
            };

            this.mockUserRepo.Setup(r => r.GetAll(It.IsAny<Expression<Func<UserEntity, bool>>>())).Returns(dbAccounts);
            this.factoryMock.Setup(m => m.GetRepository(typeof(UserEntity)))
                .Returns(this.mockUserRepo.Object);

            this.userService = new UserService(
                this.factoryMock.Object,
                this.options,
                this.mapper
            );
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
                Firstname = "John",
                Lastname = "Doe",
                Email = "someone@example.com",
                Password = BC.HashPassword(userPassword),
                Role = UserRole.ADMIN.ToString()
            };

            this.mockUserRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(dbUser);
            this.factoryMock.Setup(m => m.GetRepository(typeof(UserEntity)))
                .Returns(this.mockUserRepo.Object);

            this.userService = new UserService(
                this.factoryMock.Object,
                this.options,
                this.mapper
            );
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
                Firstname = "John",
                Lastname = "Doe",
                Email = "someone@example.com",
                Password = BC.HashPassword(userPassword),
                Role = UserRole.ADMIN.ToString()
            };

            this.mockUserRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(dbUser);
            this.factoryMock.Setup(m => m.GetRepository(typeof(UserEntity)))
                .Returns(this.mockUserRepo.Object);

            this.userService = new UserService(
                this.factoryMock.Object,
                this.options,
                this.mapper
            );
            OperationResult result = userService.DeleteUser(id);

            Assert.IsTrue(result.ResultCode == ResultCode.SUCCESS);
        }

        [TestMethod]
        public void UpdateUserCorrectly()
        {
            var updateRequest = new UpdateUserRequest
            {
                Firstname = "John",
                Lastname = "Doe",
                Password = BC.HashPassword(userPassword),
                Email = "someone@example.com",
                Role = "nonExistentRole"
            };

            this.mockUserRepo.Setup(p => p.Update(It.IsAny<UserEntity>())).Verifiable();
            this.factoryMock.Setup(m => m.GetRepository(typeof(UserEntity)))
                .Returns(this.mockUserRepo.Object);

            this.userService = new UserService(
                this.factoryMock.Object,
                this.options,
                this.mapper
            );
            OperationResult expected = userService.UpdateUser(updateRequest);

            Assert.IsTrue(expected.ResultCode == ResultCode.SUCCESS);
        }

        [TestMethod]
        public void UpdateUserThrowsError()
        {
            var updateRequest = new UpdateUserRequest
            {
                Firstname = "John",
                Lastname = "Doe",
                Password = BC.HashPassword(userPassword),
                Email = "someone@example.com",
                Role = UserRole.SELLER.ToString()
            };

            this.mockUserRepo.Setup(p => p.Update(It.IsAny<UserEntity>())).Throws(new RepositoryException());
            this.factoryMock.Setup(m => m.GetRepository(typeof(UserEntity)))
                .Returns(this.mockUserRepo.Object);

            this.userService = new UserService(
                this.factoryMock.Object,
                this.options,
                this.mapper
            );
            OperationResult expected = userService.UpdateUser(updateRequest);

            Assert.IsTrue(expected.ResultCode == ResultCode.FAIL);
        }

        [TestMethod]
        public void UpdateUserByAdmin()
        {
            var updateRequest = new UpdateUserRequest
            {
                Firstname = "John",
                Lastname = "Doe",
                Email = "someone@example.com",
                Password = BC.HashPassword(userPassword),
                Role = UserRole.SPECTATOR.ToString()
            };
            this.factoryMock.Setup(m => m.GetRepository(typeof(UserEntity)))
                .Returns(this.mockUserRepo.Object);

            this.userService = new UserService(
                this.factoryMock.Object,
                this.options,
                this.mapper
            );
            OperationResult expected = userService.UpdateUser(updateRequest, UserRole.ADMIN);

            Assert.IsTrue(expected.ResultCode == ResultCode.SUCCESS);
        }

        [TestMethod]
        public void DeleteAccountThatDoesntExist()
        {
            var id = 1;

            this.mockUserRepo.Setup(r => r.Delete(It.IsAny<int>())).Throws(new RepositoryException());
            this.factoryMock.Setup(m => m.GetRepository(typeof(UserEntity)))
                .Returns(this.mockUserRepo.Object);

            this.userService = new UserService(
                this.factoryMock.Object,
                this.options,
                this.mapper
            );
            OperationResult result = userService.DeleteUser(id);

            Assert.IsTrue(result.ResultCode == ResultCode.FAIL);
        }

        [TestMethod]
        public void GetUserByIdIsNull()
        {
            int id = 1;

            UserEntity dbUser = null;

            this.mockUserRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(dbUser);
            this.factoryMock.Setup(m => m.GetRepository(typeof(UserEntity)))
                .Returns(this.mockUserRepo.Object);

            this.userService = new UserService(
                this.factoryMock.Object,
                this.options,
                this.mapper
            );
            User account = userService.GetUser(id);

            Assert.IsNull(account);

        }
    }
}