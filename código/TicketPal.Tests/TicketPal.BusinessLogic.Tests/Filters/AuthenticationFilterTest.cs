using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TicketPal.BusinessLogic.Filters.Auth;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Models.Response;
using TicketPal.Interfaces.Factory;
using TicketPal.Interfaces.Services.Jwt;
using TicketPal.Interfaces.Services.Settings;
using TicketPal.Interfaces.Services.Users;

namespace TicketPal.BusinessLogic.Tests.Filters
{
    [TestClass]
    public class AuthenticationFilterTest
    {
        private AuthenticationFilter filter;
        // Services
        private Mock<IAppSettings> mockAppSettings;
        private Mock<IUserService> mockUserService;
        private Mock<IJwtService> mockJwtService;
        // Factory
        private Mock<IServiceFactory> mockFactoryService;

        [TestInitialize]
        public void Init()
        {
            // Factory
            this.mockFactoryService = new Mock<IServiceFactory>();
            // Services
            this.mockAppSettings = new Mock<IAppSettings>();
            this.mockUserService = new Mock<IUserService>();
            this.mockJwtService = new Mock<IJwtService>();

            // Service mock setups
            mockAppSettings.Setup(s => s.JwtSecret).Returns("someFakeSecret");
            mockJwtService.Setup(s => s.ClaimTokenValue(
                mockAppSettings.Object.JwtSecret,
                It.IsAny<string>(),
                "id"
                )).Returns("0");
            mockUserService.Setup(s => s.GetUser(It.IsAny<int>())).Returns(null as User);

            // Factory mock setups
            mockFactoryService.Setup(s => s.GetService(typeof(IAppSettings)))
            .Returns(mockAppSettings.Object);
            mockFactoryService.Setup(s => s.GetService(typeof(IJwtService)))
            .Returns(mockJwtService.Object);
        }

        [TestMethod]
        public void onFailedAuthenticationUserNotLoguedTest()
        {
            this.mockUserService.Setup(s => s.GetUser(It.IsAny<int>())).Returns(null as User);

            mockFactoryService.Setup(s => s.GetService(typeof(IUserService)))
            .Returns(mockUserService.Object);

            filter = new AuthenticationFilter(
                mockFactoryService.Object,
                new string[] { UserRole.ADMIN.ToString() }
            );

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(x => x.Request.Headers["Authorization"]).Returns(null as string);

            var actionContext = new ActionContext(
                mockHttpContext.Object,
                new Microsoft.AspNetCore.Routing.RouteData(),
                new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor() { DisplayName = "Authorization" },
                new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary()
                );

            var authContext = new AuthorizationFilterContext(actionContext, new List<IFilterMetadata>());

            filter.OnAuthorization(authContext);

            Assert.AreEqual(401, (authContext.Result as ObjectResult).StatusCode);
        }

        [TestMethod]
        public void onFailedAuthenticationUserWithNoAuthorization()
        {
            this.mockUserService.Setup(s => s.GetUser(It.IsAny<int>())).Returns(
                new User
                {
                    Id = 1,
                    Firstname = "someName",
                    Lastname = "someLastname",
                    Email = "myaccount@example.com",
                    Password = "myPassword",
                    Token = "token",
                    Role = It.IsAny<string>()
                }
            );

            mockFactoryService.Setup(s => s.GetService(typeof(IUserService)))
            .Returns(mockUserService.Object);

            filter = new AuthenticationFilter(
                mockFactoryService.Object,
                new string[] { UserRole.ADMIN.ToString() }
            );

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(x => x.Request.Headers["Authorization"]).Returns("token");

            var actionContext = new ActionContext(
                mockHttpContext.Object,
                new Microsoft.AspNetCore.Routing.RouteData(),
                new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor() { DisplayName = "Authorization" },
                new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary()
                );

            var authContext = new AuthorizationFilterContext(actionContext, new List<IFilterMetadata>());

            filter.OnAuthorization(authContext);

            Assert.AreEqual(403, (authContext.Result as ObjectResult).StatusCode);
        }

        [TestMethod]
        public void onFailedAuthenticationUserNotFound()
        {
            this.mockUserService.Setup(s => s.GetUser(It.IsAny<int>())).Returns(It.IsAny<User>());

            mockFactoryService.Setup(s => s.GetService(typeof(IUserService)))
            .Returns(mockUserService.Object);

            filter = new AuthenticationFilter(
                mockFactoryService.Object,
                new string[] { UserRole.ADMIN.ToString() }
            );

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(x => x.Request.Headers["Authorization"]).Returns("token");

            var actionContext = new ActionContext(
                mockHttpContext.Object,
                new Microsoft.AspNetCore.Routing.RouteData(),
                new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor() { DisplayName = "Authorization" },
                new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary()
                );

            var authContext = new AuthorizationFilterContext(actionContext, new List<IFilterMetadata>());

            filter.OnAuthorization(authContext);

            Assert.AreEqual(403, (authContext.Result as ObjectResult).StatusCode);
        }
    }
}