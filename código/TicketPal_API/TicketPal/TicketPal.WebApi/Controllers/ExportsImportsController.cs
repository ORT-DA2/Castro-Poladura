using Microsoft.AspNetCore.Mvc;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Models.Param;
using TicketPal.Interfaces.ExportImport;
using TicketPal.WebApi.Filters.Auth;

namespace TicketPal.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExportsImportsController: ControllerBase
    {
        IExportImportDelegator exportImportDelegator;
        public ExportsImportsController(IExportImportDelegator delegator)
        {
            this.exportImportDelegator = delegator;
        }


        [HttpGet ("formats")]
        [AuthenticationFilter(Constants.ROLE_ADMIN)]
        public IActionResult GetFormats()
        {
            return Ok(exportImportDelegator.GetFormats());
        }

        [HttpPost]
        [AuthenticationFilter(Constants.ROLE_ADMIN)]
        public IActionResult ExportImportConcerts([FromQuery] ExportImportParams param)
        {
            return Ok(exportImportDelegator.ExportImportConcerts(param));
        }


    }
}
