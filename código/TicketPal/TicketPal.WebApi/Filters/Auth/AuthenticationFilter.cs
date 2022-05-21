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
    public class AuthenticationFilter : Attribute, IAuthorizationFilter
    {
        private string[] args;
        private IUserService userService;

        public AuthFilter(string arguments = "")
        {
            this.args = arguments.Split(",");
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var token = context.HttpContext.Request.Headers["Authorization"]
                .FirstOrDefault()?.Split(" ").Last();
            this.userService = context.HttpContext.RequestServices.GetService(typeof(IUserService)) as IUserService;   
            var user = userService.RetrieveUserFromToken(token);

            if (user == null)
            {
                var unauthorizedError = new UnauthorizedError("Access denied.");
                context.Result = new ObjectResult(unauthorizedError)
                {
                    StatusCode = unauthorizedError.StatusCode
                };
            }
            else
            {                
                if (!args.Contains(user.Role))
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
