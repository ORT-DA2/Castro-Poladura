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
using System.Threading.Tasks;

namespace TicketPal.BusinessLogic.Tests.Services.Users
{
    [TestClass]
    public class UserServiceTests : BaseServiceTests
    {
        [TestMethod]
        public async Task UserAuthenticateCorrectly()
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
                Role = Constants.ROLE_ADMIN,
                ActiveAccount = true
            };

            this.mockUserRepo.Setup(r => r.Get(It.IsAny<Expression<Func<UserEntity, bool>>>()))
                .Returns(Task.FromResult(dbUser));
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
            User authenticatedUser = await userService.Login(authRequest);

            Assert.IsNotNull(authenticatedUser);
            Assert.IsNotNull(authenticatedUser.Token);
            Assert.IsTrue(authenticatedUser.Id == dbUser.Id);
        }

        [TestMethod]
        public async Task UserAuthenticatePasswordIncorrect()
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
                Role = Constants.ROLE_ADMIN,
                ActiveAccount = true
            };

            this.mockUserRepo.Setup(r => r.Get(It.IsAny<Expression<Func<UserEntity, bool>>>()))
                    .Returns(Task.FromResult(dbUser));
            this.factoryMock.Setup(m => m.GetRepository(typeof(UserEntity)))
                .Returns(this.mockUserRepo.Object);

            this.userService = new UserService(
                this.factoryMock.Object,
                this.options,
                this.mapper
            );
            User authenticatedUser = await userService.Login(authRequest);

            Assert.IsNull(authenticatedUser);
        }

        [TestMethod]
        public async Task UserAuthenticateNoUserFound()
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
                Role = Constants.ROLE_ADMIN,
                ActiveAccount = true
            };


            this.mockUserRepo.Setup(r => r.Get(It.IsAny<Expression<Func<UserEntity, bool>>>()))
                    .Returns(Task.FromResult(It.IsAny<UserEntity>()));
            this.factoryMock.Setup(m => m.GetRepository(typeof(UserEntity)))
                .Returns(this.mockUserRepo.Object);

            this.userService = new UserService(
                this.factoryMock.Object,
                this.options,
                this.mapper
            );
            User authenticatedUser = await userService.Login(authRequest);

            Assert.IsNull(authenticatedUser);
        }

        [TestMethod]
        public async Task UserRegistrationCorrect()
        {
            var signInRequest = new SignUpRequest
            {
                Firstname = "John",
                Lastname = "Doe",
                Email = "someone@example.com",
                Password = "myTestEnteredPassword",
                Role = Constants.ROLE_SPECTATOR,
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

            OperationResult result = await userService.SignUp(signInRequest);

            Assert.IsTrue(result.ResultCode == Constants.CODE_SUCCESS);
        }

        [TestMethod]
        public async Task UserRegistrationThrowsExceptionFail()
        {
            var signInRequest = new SignUpRequest
            {
                Firstname = "John",
                Lastname = "Doe",
                Email = "someone@example.com",
                Password = "myTestEnteredPassword",
                Role = Constants.ROLE_SPECTATOR
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

            OperationResult result = await userService.SignUp(signInRequest);

            Assert.IsTrue(result.ResultCode == Constants.CODE_FAIL);
        }


        [TestMethod]
        public async Task ShouldGetAllUsers()
        {
            IEnumerable<UserEntity> dbAccounts = new List<UserEntity>()
            {
                new UserEntity{Id=1,Firstname="Jennifer",Lastname="Garner",Email="user1@example.com",Role=Constants.ROLE_SPECTATOR},
                new UserEntity{Id=2,Firstname="John",Lastname="Doe",Email="user2@example.com",Role=Constants.ROLE_SPECTATOR},
                new UserEntity{Id=3,Firstname="Jane",Lastname="Doe",Email="user3@example.com",Role=Constants.ROLE_SPECTATOR},
                new UserEntity{Id=3,Firstname="Steve",Lastname="Black",Email="user4@example.com",Role=Constants.ROLE_SPECTATOR}
            };

            this.mockUserRepo.Setup(r => r.GetAll(It.IsAny<Expression<Func<UserEntity, bool>>>())).Returns(Task.FromResult(dbAccounts.ToList()));
            this.factoryMock.Setup(m => m.GetRepository(typeof(UserEntity)))
                .Returns(this.mockUserRepo.Object);

            this.userService = new UserService(
                this.factoryMock.Object,
                this.options,
                this.mapper
            );
            IEnumerable<User> result = await userService.GetUsers(Constants.ROLE_SPECTATOR);

            Assert.IsTrue(result.ToList().Count == 4);
        }

        [TestMethod]
        public async Task GetUserById()
        {
            int id = 1;
            var dbUser = new UserEntity
            {
                Id = id,
                Firstname = "John",
                Lastname = "Doe",
                Email = "someone@example.com",
                Password = BC.HashPassword(userPassword),
                Role = Constants.ROLE_SPECTATOR,
                ActiveAccount = true
            };

            this.mockUserRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(Task.FromResult(dbUser));
            this.factoryMock.Setup(m => m.GetRepository(typeof(UserEntity)))
                .Returns(this.mockUserRepo.Object);

            this.userService = new UserService(
                this.factoryMock.Object,
                this.options,
                this.mapper
            );
            User account = await userService.GetUser(id);

            Assert.IsNotNull(account);
            Assert.IsTrue(id == account.Id);
        }

        [TestMethod]
        public async Task DeleteAccountCorrectly()
        {
            var id = 1;
            var dbUser = new UserEntity
            {
                Id = id,
                Firstname = "John",
                Lastname = "Doe",
                Email = "someone@example.com",
                Password = BC.HashPassword(userPassword),
                Role = Constants.ROLE_ADMIN,
                ActiveAccount = true
            };

            this.mockUserRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(Task.FromResult(dbUser));
            this.factoryMock.Setup(m => m.GetRepository(typeof(UserEntity)))
                .Returns(this.mockUserRepo.Object);

            this.userService = new UserService(
                this.factoryMock.Object,
                this.options,
                this.mapper
            );
            OperationResult result = await userService.DeleteUser(id);

            Assert.IsTrue(result.ResultCode == Constants.CODE_SUCCESS);
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
            OperationResult expected = userService.UpdateUser(updateRequest, "SPECTATOR");

            Assert.IsTrue(expected.ResultCode == Constants.CODE_SUCCESS);
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
                Role = Constants.ROLE_SELLER
            };

            this.mockUserRepo.Setup(p => p.Update(It.IsAny<UserEntity>())).Throws(new RepositoryException());
            this.factoryMock.Setup(m => m.GetRepository(typeof(UserEntity)))
                .Returns(this.mockUserRepo.Object);

            this.userService = new UserService(
                this.factoryMock.Object,
                this.options,
                this.mapper
            );
            OperationResult expected = userService.UpdateUser(updateRequest, "SPECTATOR");

            Assert.IsTrue(expected.ResultCode == Constants.CODE_FAIL);
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
                Role = Constants.ROLE_SPECTATOR
            };
            this.factoryMock.Setup(m => m.GetRepository(typeof(UserEntity)))
                .Returns(this.mockUserRepo.Object);

            this.userService = new UserService(
                this.factoryMock.Object,
                this.options,
                this.mapper
            );
            OperationResult expected = userService.UpdateUser(updateRequest, "SPECTATOR");

            Assert.IsTrue(expected.ResultCode == Constants.CODE_SUCCESS);
        }

        [TestMethod]
        public async Task DeleteAccountThatDoesntExist()
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
            OperationResult result = await userService.DeleteUser(id);

            Assert.IsTrue(result.ResultCode == Constants.CODE_FAIL);
        }

        [TestMethod]
        public async Task GetUserByIdIsNull()
        {
            int id = 1;

            UserEntity dbUser = null;

            this.mockUserRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(Task.FromResult(dbUser));
            this.factoryMock.Setup(m => m.GetRepository(typeof(UserEntity)))
                .Returns(this.mockUserRepo.Object);

            this.userService = new UserService(
                this.factoryMock.Object,
                this.options,
                this.mapper
            );
            User account = await userService.GetUser(id);

            Assert.IsNull(account);

        }

        [TestMethod]
        public async Task RetrieveUserFromTokenIsNullToken()
        {
            this.userService = new UserService(
                this.factoryMock.Object,
                this.options,
                this.mapper
            );

            var user = await userService.RetrieveUserFromToken(null);
            Assert.IsNull(user);
        }

        [TestMethod]
        public async Task RetrieveUserFromTokenClaimNull()
        {
            this.mockJwtService.Setup(m => m.ClaimTokenValue(
                It.IsAny<string>(),
                It.IsAny<string>(),
                "id"
            )).Returns(null as string);

            this.factoryMock.Setup(m => m.GetService(typeof(IJwtService)))
                .Returns(this.mockJwtService.Object);

            this.userService = new UserService(
                this.factoryMock.Object,
                this.options,
                this.mapper
            );
            var user = await userService.RetrieveUserFromToken("someToken");
            Assert.IsNull(user);
        }

        [TestMethod]
        public async Task RetrieveUserFromTokenOk()
        {
            var dbUser = new UserEntity
            {
                Id = 1,
                Firstname = "John",
                Lastname = "Doe",
                Email = "someone@example.com",
                Password = BC.HashPassword(userPassword),
                Role = Constants.ROLE_ADMIN,
                ActiveAccount = true
            };

            this.mockJwtService.Setup(m => m.ClaimTokenValue(
                It.IsAny<string>(),
                It.IsAny<string>(),
                "id"
            )).Returns("1");

            this.mockUserRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(Task.FromResult(dbUser));
            this.factoryMock.Setup(m => m.GetRepository(typeof(UserEntity)))
                .Returns(this.mockUserRepo.Object);
            this.factoryMock.Setup(m => m.GetService(typeof(IJwtService)))
                .Returns(this.mockJwtService.Object);

            this.userService = new UserService(
                this.factoryMock.Object,
                this.options,
                this.mapper
            );
            var user = await userService.RetrieveUserFromToken("someToken");
            Assert.IsNotNull(user);
        }
    }
}