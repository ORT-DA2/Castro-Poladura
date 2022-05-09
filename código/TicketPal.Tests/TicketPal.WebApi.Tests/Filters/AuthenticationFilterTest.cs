using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TicketPal.BusinessLogic.Services.Settings;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Models.Response;
using TicketPal.Interfaces.Factory;
using TicketPal.Interfaces.Services.Jwt;
using TicketPal.Interfaces.Services.Settings;
using TicketPal.Interfaces.Services.Users;
using TicketPal.WebApi.Filters.Auth;

namespace TicketPal.WebApi.Tests.Filters
{
    [TestClass]
    public class AuthenticationFilterTest
    {
        private AuthFilter filter;
        // Services
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
            this.mockUserService = new Mock<IUserService>();
            this.mockJwtService = new Mock<IJwtService>();

            // Service mock setups
            mockJwtService.Setup(s => s.ClaimTokenValue(
                new AppSettings().JwtSecret,
                It.IsAny<string>(),
                "id"
                )).Returns("0");
            mockUserService.Setup(s => s.GetUser(It.IsAny<int>())).Returns(null as User);

            // Factory mock setups
            mockFactoryService.Setup(s => s.GetService(typeof(IJwtService)))
            .Returns(mockJwtService.Object);
        }

        [TestMethod]
        public void onFailedAuthenticationUserNotLoguedTest()
        {
            this.mockUserService.Setup(s => s.GetUser(It.IsAny<int>())).Returns(null as User);

            mockFactoryService.Setup(s => s.GetService(typeof(IUserService)))
            .Returns(mockUserService.Object);

            filter = new AuthFilter(
                UserRole.ADMIN.ToString()
            );

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(x => x.Request.Headers["Authorization"]).Returns(null as string);
            mockHttpContext.Setup(x => x.RequestServices.GetService(typeof(IServiceFactory)))
                .Returns(mockFactoryService.Object);

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
                    Role = ""
                }
            );

            mockFactoryService.Setup(s => s.GetService(typeof(IUserService)))
            .Returns(mockUserService.Object);

            filter = new AuthFilter(
                UserRole.ADMIN.ToString()
            );

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(x => x.Request.Headers["Authorization"]).Returns("token");
            mockHttpContext.Setup(x => x.RequestServices.GetService(typeof(IServiceFactory)))
                .Returns(mockFactoryService.Object);

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
        public void onFailedAuthenticationUserWithWrongRole()
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
                    Role = UserRole.SPECTATOR.ToString()
                }
            );

            mockFactoryService.Setup(s => s.GetService(typeof(IUserService)))
            .Returns(mockUserService.Object);

            filter = new AuthFilter(
                UserRole.ADMIN.ToString()
            );

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(x => x.Request.Headers["Authorization"]).Returns("token");
            mockHttpContext.Setup(x => x.RequestServices.GetService(typeof(IServiceFactory)))
                .Returns(mockFactoryService.Object);

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

            filter = new AuthFilter();

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(x => x.Request.Headers["Authorization"]).Returns("token");
            mockHttpContext.Setup(x => x.RequestServices.GetService(typeof(IServiceFactory)))
                .Returns(mockFactoryService.Object);

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