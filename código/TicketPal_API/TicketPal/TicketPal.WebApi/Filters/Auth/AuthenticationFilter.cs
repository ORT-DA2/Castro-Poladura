using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using TicketPal.Domain.Models.Response.Error;
using TicketPal.Interfaces.Services.Users;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace TicketPal.WebApi.Filters.Auth
{
    public class AuthenticationFilter : Attribute, IAsyncAuthorizationFilter
    {
        private string[] args;
        private IUserService userService;

        public AuthenticationFilter(string arguments = "")
        {
            this.args = arguments.Split(",");
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {

            var token = context.HttpContext.Request.Headers["Authorization"]
                .FirstOrDefault()?.Split(" ").Last();
            this.userService = context.HttpContext.RequestServices.GetService(typeof(IUserService)) as IUserService;
            var user = await userService.RetrieveUserFromToken(token);

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
                else
                {
                    var json = JsonConvert.SerializeObject(user);
                    context.HttpContext.Session.SetString("user", json);
                }
            }
        }
    }
}
