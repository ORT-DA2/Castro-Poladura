using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TicketPal.Domain.Models.Response.Error;

namespace TicketPal.WebApi.Filters.Model
{
    public class ModelValidateFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errorMessages = context.ModelState.Values
                    .Aggregate(new List<string>(), (a, c) =>
                    {
                        a.AddRange(c.Errors.Select(r => r.ErrorMessage));
                        return a;
                    },
                     a => a);

                context.Result = new BadRequestObjectResult(
                    new BadRequestError(
                        string.Format(string.Join(" ", errorMessages))));
            }
        }
    }
}