using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TicketPal.Domain.Models.Response.Error;
using TicketPal.Interfaces.Factory;
using TicketPal.Interfaces.Services.Jwt;
using TicketPal.Interfaces.Services.Settings;
using TicketPal.Interfaces.Services.Users;

namespace TicketPal.BusinessLogic.Filters.Auth
{
    public class AuthenticationFilter : Attribute, IAuthorizationFilter
    {
        private string[] roleArgs;
        private IUserService userService;
        private IAppSettings settingsService;
        private IJwtService jwtService;

        public AuthenticationFilter(IServiceFactory factory, string[] args)
        {
            roleArgs = args;
            this.settingsService = factory.GetService(typeof(IAppSettings)) as IAppSettings;
            this.userService = factory.GetService(typeof(IUserService)) as IUserService;
            this.jwtService = factory.GetService(typeof(IJwtService)) as IJwtService;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
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

                if (user == null || !roleArgs.Contains(user.Role))
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
