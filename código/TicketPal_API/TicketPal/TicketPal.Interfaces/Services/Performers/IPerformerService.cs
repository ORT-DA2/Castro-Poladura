using System.Collections.Generic;
using TicketPal.Domain.Models.Request;
using TicketPal.Domain.Models.Response;

namespace TicketPal.Interfaces.Services.Performers
{
    public interface IPerformerService
    {
        OperationResult AddPerformer(AddPerformerRequest model);
        OperationResult UpdatePerformer(UpdatePerformerRequest model);
        OperationResult DeletePerformer(int id);
        IEnumerable<Performer> GetPerformers();
        Performer GetPerformer(int id);
    }
}
