using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TicketPal.BusinessLogic.Services.Settings;
using TicketPal.Domain.Models.Response.Error;
using TicketPal.Interfaces.Factory;
using TicketPal.Interfaces.Services.Jwt;
using TicketPal.Interfaces.Services.Users;

namespace TicketPal.WebApi.Filters.Auth
{
    public class AuthFilter : Attribute, IAuthorizationFilter
    {
        private string[] args;
        private IUserService userService;
        private AppSettings settingsService;
        private IJwtService jwtService;

        public AuthFilter(string arguments = "")
        {
            this.args = arguments.Split(",");
            this.settingsService = new AppSettings();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var factory = context.HttpContext.RequestServices.GetService(typeof(IServiceFactory)) as IServiceFactory;

            this.userService = factory.GetService(typeof(IUserService)) as IUserService;
            this.jwtService = factory.GetService(typeof(IJwtService)) as IJwtService;

            var token = context.HttpContext.Request.Headers["Authorization"]
                .FirstOrDefault()?.Split(" ").Last();

            if (String.IsNullOrEmpty(token))
            {
                var unauthorizedError = new UnauthorizedError("Access denied.");
                context.Result = new ObjectResult(unauthorizedError)
                {
                    StatusCode = unauthorizedError.StatusCode
                };
            }
            else
            {
                var accountId = int.Parse(jwtService.ClaimTokenValue(settingsService.JwtSecret, token, "id"));
                var user = userService.GetUser(accountId);

                if (user == null || !args.Contains(user.Role))
                {
                    var forbidden = new ForbiddenError("No required authorization to access resource.");
                    context.Result = new ObjectResult(forbidden)
                    {
                        StatusCode = forbidden.StatusCode
                    };
                }
            }

        }
    }
}
