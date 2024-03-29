using System.Collections.Generic;
using System.Threading.Tasks;
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
using TicketPal.Interfaces.Services.Users;
using TicketPal.WebApi.Filters.Auth;

namespace TicketPal.WebApi.Tests.Filters
{
    [TestClass]
    public class AuthenticationFilterTest
    {
        private AuthenticationFilter filter;
        // Mock services
        private Mock<IUserService> mockUserService;

        [TestInitialize]
        public void Init()
        {
            // Services mocks
            this.mockUserService = new Mock<IUserService>();
            mockUserService.Setup(s => s.GetUser(It.IsAny<int>())).Returns(Task.FromResult(null as User));
        }

        [TestMethod]
        public async Task onFailedAuthenticationUserNotLoguedTest()
        {
            this.mockUserService.Setup(s => s.RetrieveUserFromToken(It.IsAny<string>())).Returns(Task.FromResult(null as User));

            filter = new AuthenticationFilter(
                Constants.ROLE_ADMIN
            );

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(x => x.Request.Headers["Authorization"]).Returns(null as string);
            mockHttpContext.Setup(x => x.RequestServices.GetService(typeof(IUserService)))
                .Returns(mockUserService.Object);

            var actionContext = new ActionContext(
                mockHttpContext.Object,
                new Microsoft.AspNetCore.Routing.RouteData(),
                new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor() { DisplayName = "Authorization" },
                new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary()
                );

            var authContext = new AuthorizationFilterContext(actionContext, new List<IFilterMetadata>());

            await filter.OnAuthorizationAsync(authContext);

            Assert.AreEqual(401, (authContext.Result as ObjectResult).StatusCode);
        }

        [TestMethod]
        public async Task onFailedAuthenticationUserWithNoAuthorization()
        {
            this.mockUserService.Setup(s => s.RetrieveUserFromToken(It.IsAny<string>())).Returns(
                Task.FromResult(
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
            ));

            filter = new AuthenticationFilter(
                Constants.ROLE_ADMIN
            );

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(x => x.Request.Headers["Authorization"]).Returns("token");
            mockHttpContext.Setup(x => x.RequestServices.GetService(typeof(IUserService)))
                .Returns(mockUserService.Object);

            var actionContext = new ActionContext(
                mockHttpContext.Object,
                new Microsoft.AspNetCore.Routing.RouteData(),
                new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor() { DisplayName = "Authorization" },
                new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary()
                );

            var authContext = new AuthorizationFilterContext(actionContext, new List<IFilterMetadata>());

            await filter.OnAuthorizationAsync(authContext);

            Assert.AreEqual(403, (authContext.Result as ObjectResult).StatusCode);
        }
    }
}