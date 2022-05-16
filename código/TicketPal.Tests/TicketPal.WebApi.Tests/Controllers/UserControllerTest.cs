using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Models.Request;
using TicketPal.Domain.Models.Response;
using TicketPal.Interfaces.Factory;
using TicketPal.Interfaces.Services.Jwt;
using TicketPal.Interfaces.Services.Users;
using TicketPal.WebApi.Controllers;

namespace TicketPal.WebApi.Tests.Controllers
{
    [TestClass]
    public class UserControllerTest
    {
        private Mock<IUserService> mockService;
        private List<User> users;
        private UsersController controller;

        [TestInitialize]
        public void TestSetup()
        {
            mockService = new Mock<IUserService>(MockBehavior.Default);
            controller = new UsersController(mockService.Object);
            this.users = SetupUsers();
        }

        [TestMethod]
        public void UserLoginIsCorrect()
        {
            var request = new AuthenticationRequest
            {
                Email = users[0].Email,
                Password = users[0].Password
            };

            mockService.Setup(s => s.Login(request)).Returns(users[0]);

            var result = controller.Authenticate(request);

            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        public void UserSignUpOk()
        {
            var request = new SignUpRequest
            {
                Firstname = users[1].Firstname,
                Lastname = users[1].Lastname,
                Email = users[1].Email,
                Password = users[1].Password,
                Role = users[1].Role
            };

            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_SUCCESS,
                Message = "User registered successfully"
            };

            mockService.Setup(s => s.SignUp(request)).Returns(operationResult);

            var result = controller.Register(request);

            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public void RegisterUserBadRequest()
        {
            var request = new SignUpRequest
            {
                Firstname = users[0].Firstname,
                Lastname = users[0].Lastname,
                Email = users[0].Email,
                Password = users[0].Password,
                Role = "someRole"
            };

            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_FAIL,
                Message = "Some error message"
            };

            mockService.Setup(s => s.SignUp(request)).Returns(operationResult);

            var result = controller.Register(request);

            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(400, statusCode);
        }

        [TestMethod]
        public void GetAnyAccountAdminOk()
        {
            var mockHttpContext = new Mock<HttpContext>();
            var mockHeaderHttp = new Mock<IHeaderDictionary>();
            mockHeaderHttp.Setup(x => x[It.IsAny<string>()]).Returns("someHeader");
            var mockHttpRequest = new Mock<HttpRequest>();
            mockHttpRequest.Setup(s => s.Headers).Returns(mockHeaderHttp.Object);
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockHttpContext.Setup(s => s.Request).Returns(mockHttpRequest.Object);
            
            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            mockService.Setup(s => s.RetrieveUserFromToken(It.IsAny<string>())).Returns(users[0]);

            var account = controller.GetUserAccount(users[1].Id);

            var objectResult = account as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public void GetAccountIfSameAccountOk()
        {
            var mockHttpContext = new Mock<HttpContext>();
            var mockHeaderHttp = new Mock<IHeaderDictionary>();
            mockHeaderHttp.Setup(x => x[It.IsAny<string>()]).Returns("someHeader");
            var mockHttpRequest = new Mock<HttpRequest>();
            mockHttpRequest.Setup(s => s.Headers).Returns(mockHeaderHttp.Object);
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockHttpContext.Setup(s => s.Request).Returns(mockHttpRequest.Object);
            
            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            mockService.Setup(s => s.RetrieveUserFromToken(It.IsAny<string>())).Returns(users[3]);

            var account = controller.GetUserAccount(users[3].Id);

            var objectResult = account as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public void GetAnyAccountsOk()
        {
            mockService.Setup(s => s.GetUsers(Constants.ROLE_SPECTATOR)).Returns(users);

            var account = controller.GetUserAccounts(Constants.ROLE_SPECTATOR);

            var objectResult = account as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public void UpdateAccountOk()
        {
            var mockHttpContext = new Mock<HttpContext>();
            var mockHeaderHttp = new Mock<IHeaderDictionary>();
            mockHeaderHttp.Setup(x => x[It.IsAny<string>()]).Returns("someHeader");
            var mockHttpRequest = new Mock<HttpRequest>();
            mockHttpRequest.Setup(s => s.Headers).Returns(mockHeaderHttp.Object);
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockHttpContext.Setup(s => s.Request).Returns(mockHttpRequest.Object);
            
            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            var updateRequest = new UpdateUserRequest
            {
                Id = 4,
                Firstname = "someName",
                Lastname = "someLastName",
                Email = "someEmail",
                Password = "somePassword",
                Role = Constants.ROLE_SPECTATOR
            };

            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_SUCCESS,
                Message = "User updated successfully"
            };

            mockService.Setup(s => s.RetrieveUserFromToken(It.IsAny<string>())).Returns(users[3]);
            mockService.Setup(s => s.UpdateUser(updateRequest, It.IsAny<string>()))
                .Returns(operationResult);

            var account = controller.Update(users[3].Id, updateRequest);

            var objectResult = account as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public void UpdateSpectatorUserAccountBySameUserOk()
        {
            var mockHttpContext = new Mock<HttpContext>();
            var mockHeaderHttp = new Mock<IHeaderDictionary>();
            mockHeaderHttp.Setup(x => x[It.IsAny<string>()]).Returns("someHeader");
            var mockHttpRequest = new Mock<HttpRequest>();
            mockHttpRequest.Setup(s => s.Headers).Returns(mockHeaderHttp.Object);
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockHttpContext.Setup(s => s.Request).Returns(mockHttpRequest.Object);            
            
            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            var updateRequest = new UpdateUserRequest
            {
                Firstname = "someName",
                Lastname = "someLastName",
                Email = users[3].Email,
                Password = "somePassword",
                Role = users[3].Role
            };

            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_SUCCESS,
                Message = "User updated successfully"
            };

            mockService.Setup(s => s.RetrieveUserFromToken(It.IsAny<string>())).Returns(users[3]);
            mockService.Setup(s => s.UpdateUser(updateRequest, It.IsAny<string>()))
                .Returns(operationResult);

            var account = controller.Update(users[3].Id, updateRequest);

            var objectResult = account as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public void UpdateUserAccountBadRequest()
        {
            var mockHttpContext = new Mock<HttpContext>();
            var mockHeaderHttp = new Mock<IHeaderDictionary>();
            mockHeaderHttp.Setup(x => x[It.IsAny<string>()]).Returns("someHeader");
            var mockHttpRequest = new Mock<HttpRequest>();
            mockHttpRequest.Setup(s => s.Headers).Returns(mockHeaderHttp.Object);
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockHttpContext.Setup(s => s.Request).Returns(mockHttpRequest.Object);            
            
            controller.ControllerContext.HttpContext = mockHttpContext.Object;
            
            var updateRequest = new UpdateUserRequest
            {
                Firstname = "someName",
                Lastname = "someLastName",
                Email = users[4].Email,
                Password = "somePassword",
                Role = users[4].Role
            };

            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_FAIL,
                Message = "some error message"
            };

            mockService.Setup(s => s.RetrieveUserFromToken(It.IsAny<string>())).Returns(users[4]);
            mockService.Setup(s => s.UpdateUser(updateRequest, It.IsAny<string>()))
                .Returns(operationResult);

            var account = controller.Update(users[4].Id, updateRequest);

            var objectResult = account as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(400, statusCode);
        }

        [TestMethod]
        public void DeleteAccountOk()
        {
            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_SUCCESS,
                Message = "User updated successfully"
            };

            mockService.Setup(s => s.DeleteUser(It.IsAny<int>())).Returns(operationResult);

            var account = controller.DeleteAccount(It.IsAny<int>());

            var objectResult = account as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public void DeleteAccountBadRequest()
        {
            var operationResult = new OperationResult
            {
                ResultCode = Constants.CODE_FAIL,
                Message = "some error message"
            };

            mockService.Setup(s => s.DeleteUser(It.IsAny<int>())).Returns(operationResult);

            var account = controller.DeleteAccount(It.IsAny<int>());

            var objectResult = account as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(400, statusCode);

        }

        private List<User> SetupUsers()
        {
            return new List<User>
            {
                new User
                {
                    Id = 1,
                    Firstname = "someName1",
                    Lastname = "someLastname1",
                    Email = "myaccount1@example.com",
                    Password = "myPassword1",
                    Token = "token1",
                    Role = Constants.ROLE_ADMIN
                },
                new User
                {
                    Id = 2,
                    Firstname = "someName2",
                    Lastname = "someLastname2",
                    Email = "myaccount2@example.com",
                    Password = "myPassword2",
                    Token = "token2",
                    Role = Constants.ROLE_SELLER
                },
                new User
                {
                    Id = 3,
                    Firstname = "someName3",
                    Lastname = "someLastname3",
                    Email = "myaccount3@example.com",
                    Password = "myPassword3",
                    Token = "token3",
                    Role = Constants.ROLES_SUPERVISOR
                },
                new User
                {
                    Id = 4,
                    Firstname = "someName4",
                    Lastname = "someLastname4",
                    Email = "myaccount4@example.com",
                    Password = "myPassword4",
                    Token = "token4",
                    Role = Constants.ROLE_SPECTATOR
                },
                new User
                {
                    Id = 5,
                    Firstname = "someName5",
                    Lastname = "someLastname5",
                    Email = "myaccount5@example.com",
                    Password = "myPassword5",
                    Token = "token5",
                    Role = Constants.ROLES_SUPERVISOR
                }
            };
        }
    }
}