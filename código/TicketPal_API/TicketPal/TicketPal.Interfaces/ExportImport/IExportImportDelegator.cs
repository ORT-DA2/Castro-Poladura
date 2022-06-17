using System;
using System.Collections.Generic;
using TicketPal.Domain.Models.Param;
using TicketPal.Domain.Models.Response;

namespace TicketPal.Interfaces.ExportImport
{
    public interface IExportImportDelegator
    {
        List<string> GetFormats();
        OperationResult ExportImportConcerts(ExportImportParams param);
        IEnumerable<Type> GetAllTypes(Type genericType);
    }
}
