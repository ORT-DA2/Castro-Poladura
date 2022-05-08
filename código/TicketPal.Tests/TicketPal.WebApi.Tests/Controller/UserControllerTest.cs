using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Models.Request;
using TicketPal.Domain.Models.Response;
using TicketPal.Interfaces.Services.Users;

namespace TicketPal.WebApi.Tests.Controller
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

        [TestMethod]
        public void UserLoginNotAuthenticated()
        {
            var request = new AuthenticationRequest
            {
                Email = users[1].Email,
                Password = users[1].Password
            };

            mockService.Setup(s => s.Login(request)).Returns(It.IsAny<User>());

            var result = controller.Authenticate(request);

            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(401, statusCode);
        }

        [TestMethod]
        public void UserSpectatorSignUpOk()
        {
            var request = new SignUpRequest
            {
                Firstname = users[3].Firstname,
                Lastname = users[3].Lastname,
                Email = users[3].Email,
                Password = users[3].Password,
                Role = users[3].Role
            };

            var operationResult = new OperationResult
            {
                ResultCode = ResultCode.SUCCESS,
                Message = "User registered successfully"
            };

            mockService.Setup(s => s.SignUp(request)).Returns(operationResult);

            var result = controller.Register(request);

            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(201, statusCode);
        }

        public void UserSignUpSellerByAdminOk()
        {
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Items["User"] = users[0];

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
                ResultCode = ResultCode.SUCCESS,
                Message = "User registered successfully"
            };

            mockService.Setup(s => s.SignUp(request)).Returns(operationResult);

            var result = controller.Register(request);

            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(201, statusCode);
        }

        public void UserSignUpSpectatorByAdminOk()
        {
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Items["User"] = users[0];

            var request = new SignUpRequest
            {
                Firstname = users[4].Firstname,
                Lastname = users[4].Lastname,
                Email = users[4].Email,
                Password = users[4].Password,
                Role = users[4].Role
            };

            var operationResult = new OperationResult
            {
                ResultCode = ResultCode.SUCCESS,
                Message = "User registered successfully"
            };

            mockService.Setup(s => s.SignUp(request)).Returns(operationResult);

            var result = controller.Register(request);

            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(201, statusCode);
        }

        public void UserSignUpSupervisorByAdminOk()
        {
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Items["User"] = users[0];

            var request = new SignUpRequest
            {
                Firstname = users[2].Firstname,
                Lastname = users[2].Lastname,
                Email = users[2].Email,
                Password = users[2].Password,
                Role = users[2].Role
            };

            var operationResult = new OperationResult
            {
                ResultCode = ResultCode.SUCCESS,
                Message = "User registered successfully"
            };

            mockService.Setup(s => s.SignUp(request)).Returns(operationResult);

            var result = controller.Register(request);

            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(201, statusCode);
        }

        public void UserSignUpAdminByAdminOk()
        {
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Items["User"] = users[0];

            var request = new SignUpRequest
            {
                Firstname = users[0].Firstname,
                Lastname = users[0].Lastname,
                Email = users[0].Email,
                Password = users[0].Password,
                Role = users[0].Role
            };

            var operationResult = new OperationResult
            {
                ResultCode = ResultCode.SUCCESS,
                Message = "User registered successfully"
            };

            mockService.Setup(s => s.SignUp(request)).Returns(operationResult);

            var result = controller.Register(request);

            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(201, statusCode);
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
                ResultCode = ResultCode.FAIL,
                Message = "Some error message"
            };

            mockService.Setup(s => s.SignUp(request)).Returns(operationResult);

            var result = controller.Register(request);

            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(400, statusCode);
        }

        [TestMethod]
        public void GetAnyAccountIfIsAdminOk()
        {
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Items["User"] = users[0];

            mockService.Setup(s => s.GetUser(It.IsAny<int>())).Returns(users[2]);

            var account = controller.GetUserAccount(It.IsAny<int>());

            var objectResult = account as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public void GetAccountIfSameAccountOk()
        {
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Items["User"] = users[3];

            mockService.Setup(s => s.GetUser(It.IsAny<int>())).Returns(users[3]);

            var account = controller.GetUserAccount(It.IsAny<int>());

            var objectResult = account as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public void GetAnyAccountNotAdminUserError()
        {
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            controller.ControllerContext.HttpContext.Items["User"] =
                new User
                {
                    Id = 0,
                    Firstname = "someName",
                    Lastname = "someLastname",
                    Email = "myaccount@example.com",
                    Password = "myPassword",
                    Token = "token5",
                    Role = It.IsAny<string>()
                };

            mockService.Setup(s => s.GetUser(It.IsAny<int>())).Returns(users[3]);

            var account = controller.GetUserAccount(It.IsAny<int>());

            var objectResult = account as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public void GetAnyAccountsIfIsAdminOk()
        {
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Items["User"] = users[0];

            mockService.Setup(s => s.GetUsers(UserRole.SPECTATOR)).Returns(users);

            var account = controller.GetAccounts(UserRole.SPECTATOR.ToString());

            var objectResult = account as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public void GetAnyAccountsUserNotAuthorized()
        {
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Items["User"] =
                new User
                {
                    Id = 0,
                    Firstname = "someName",
                    Lastname = "someLastname",
                    Email = "myaccount@example.com",
                    Password = "myPassword",
                    Token = "token5",
                    Role = It.IsAny<string>()
                };
            var account = controller.GetAccounts(UserRole.SPECTATOR.ToString());

            var objectResult = account as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(403, statusCode);
        }

        [TestMethod]
        public void GetAnyAccountsUserNotInContext()
        {
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var account = controller.GetAccounts(UserRole.SPECTATOR.ToString());

            var objectResult = account as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(403, statusCode);
        }

        [TestMethod]
        public void RegisterAnyNoSpectatorAccountWithNoAdminAuthorization()
        {
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Items["User"] =
                new User
                {
                    Id = 0,
                    Firstname = "someName",
                    Lastname = "someLastname",
                    Email = "myaccount@example.com",
                    Password = "myPassword",
                    Token = "token5",
                    Role = It.IsAny<string>()
                };

            var request = new SignUpRequest
            {
                Firstname = "someName",
                Lastname = "someLastName",
                Email = "someEmail",
                Password = "somePassword",
                Role = It.IsAny<string>()
            };

            var result = controller.Register(request);

            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(403, statusCode);
        }

        [TestMethod]
        public void RegisterNoSpectatorUserWithNoUserInContext()
        {
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var request = new SignUpRequest
            {
                Firstname = users[4].Firstname,
                Lastname = users[4].Lastname,
                Email = users[4].Email,
                Password = users[4].Password,
                Role = users[4].Role
            };


            var result = controller.Register(request);

            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(403, statusCode);
        }

        [TestMethod]
        public void UpdateAnyAccountByAdminOk()
        {
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Items["User"] = users[0];

            var updateRequest = new UpdateUserRequest
            {
                Firstname = "someName",
                Lastname = "someLastName",
                Email = "someEmail",
                Password = "somePassword",
                Role = It.IsAny<string>()
            };

            var operationResult = new OperationResult
            {
                ResultCode = ResultCode.SUCCESS,
                Message = "User updated successfully"
            };

            mockService.Setup(s => s.GetUser(It.IsAny<int>())).Returns(users[3]);
            mockService.Setup(s => s.UpdateUser(updateRequest, It.IsAny<UserRole>()))
                .Returns(operationResult);

            var account = controller.Update(users[3].Id, updateRequest);

            var objectResult = account as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public void UpdateSpectatorUserAccountBySameUserOk()
        {
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Items["User"] = users[4];

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
                ResultCode = ResultCode.SUCCESS,
                Message = "User updated successfully"
            };

            mockService.Setup(s => s.GetUser(It.IsAny<int>())).Returns(users[4]);
            mockService.Setup(s => s.UpdateUser(updateRequest, It.IsAny<UserRole>()))
                .Returns(operationResult);

            var account = controller.Update(users[4].Id, updateRequest);

            var objectResult = account as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public void UpdateAnyUserAccountByNotAuthorizedUser()
        {
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Items["User"] =
            new User
            {
                Id = 0,
                Firstname = "someName",
                Lastname = "someLastname",
                Email = "myaccount@example.com",
                Password = "myPassword",
                Token = "token5",
                Role = It.IsAny<string>()
            };

            var updateRequest = new UpdateUserRequest
            {
                Firstname = "someName",
                Lastname = "someLastName",
                Email = users[4].Email,
                Password = "somePassword",
                Role = users[4].Role
            };

            var account = controller.Update(users[4].Id, updateRequest);

            var objectResult = account as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(403, statusCode);
        }

        [TestMethod]
        public void UpdateAnyUserAccountUserNotInContext()
        {
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var updateRequest = new UpdateUserRequest
            {
                Firstname = "someName",
                Lastname = "someLastName",
                Email = users[4].Email,
                Password = "somePassword",
                Role = users[4].Role
            };

            var account = controller.Update(users[4].Id, updateRequest);

            var objectResult = account as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(403, statusCode);
        }

        [TestMethod]
        public void UpdateUserAccountBadRequest()
        {
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Items["User"] = users[4];

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
                ResultCode = ResultCode.FAIL,
                Message = "some error message"
            };

            mockService.Setup(s => s.GetUser(It.IsAny<int>())).Returns(users[4]);
            mockService.Setup(s => s.UpdateUser(updateRequest, It.IsAny<UserRole>()))
                .Returns(operationResult);

            var account = controller.Update(users[4].Id, updateRequest);

            var objectResult = account as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(400, statusCode);
        }

        [TestMethod]
        public void DeleteAnyAccountIfAdminOk()
        {
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Items["User"] = users[0];

            var operationResult = new OperationResult
            {
                ResultCode = ResultCode.SUCCESS,
                Message = "User updated successfully"
            };

            mockService.Setup(s => s.DeleteUser(It.IsAny<int>())).Returns(operationResult);

            var account = controller.DeleteAccount(It.IsAny<int>());

            var objectResult = account as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public void DeleteAnyAccountByNoAdmin()
        {
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Items["User"] =
                new User
                {
                    Id = 0,
                    Firstname = "someName",
                    Lastname = "someLastname",
                    Email = "myaccount@example.com",
                    Password = "myPassword",
                    Token = "token5",
                    Role = It.IsAny<string>()
                };

            var account = controller.DeleteAccount(It.IsAny<int>());

            var objectResult = account as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(403, statusCode);
        }


        [TestMethod]
        public void DeleteAnyAccountByNoUserInContext()
        {
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var account = controller.DeleteAccount(It.IsAny<int>());

            var objectResult = account as ObjectResult;
            var statusCode = objectResult.StatusCode;

            Assert.AreEqual(403, statusCode);
        }

        [TestMethod]
        public void DeleteAccountBadRequest()
        {
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Items["User"] = users[0];

            var operationResult = new OperationResult
            {
                ResultCode = ResultCode.FAIL,
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
                    Role = UserRole.ADMIN.ToString()
                },
                new User
                {
                    Id = 2,
                    Firstname = "someName2",
                    Lastname = "someLastname2",
                    Email = "myaccount2@example.com",
                    Password = "myPassword2",
                    Token = "token2",
                    Role = UserRole.SELLER.ToString()
                },
                new User
                {
                    Id = 3,
                    Firstname = "someName3",
                    Lastname = "someLastname3",
                    Email = "myaccount3@example.com",
                    Password = "myPassword3",
                    Token = "token3",
                    Role = UserRole.SUPERVISOR.ToString()
                },
                new User
                {
                    Id = 4,
                    Firstname = "someName4",
                    Lastname = "someLastname4",
                    Email = "myaccount4@example.com",
                    Password = "myPassword4",
                    Token = "token4",
                    Role = UserRole.SPECTATOR.ToString()
                },
                new User
                {
                    Id = 5,
                    Firstname = "someName5",
                    Lastname = "someLastname5",
                    Email = "myaccount5@example.com",
                    Password = "myPassword5",
                    Token = "token5",
                    Role = UserRole.SUPERVISOR.ToString()
                }
            };
        }
    }
}