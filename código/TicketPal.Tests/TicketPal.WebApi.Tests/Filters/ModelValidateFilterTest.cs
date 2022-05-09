using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TicketPal.WebApi.Filters.Model;

namespace TicketPal.WebApi.Tests.Filters
{
    [TestClass]
    public class ModelValidatFilterTest
    {
        private ModelValidateFilter filter;

        [TestMethod]
        public void EnsureCreatedNotNull()
        {
            filter = new ModelValidateFilter();
            var modelState = new ModelStateDictionary();
            modelState.AddModelError("name", "invalid");

            var actionContext = new ActionContext(
                Mock.Of<HttpContext>(),
                new Microsoft.AspNetCore.Routing.RouteData(),
                new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor(),
                modelState);

            var actionExecutingContext = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                Mock.Of<Controller>()
            );

            filter.OnActionExecuting(actionExecutingContext);
        }

    }
}