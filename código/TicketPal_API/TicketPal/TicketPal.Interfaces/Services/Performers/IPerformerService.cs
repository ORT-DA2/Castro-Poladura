using System.Collections.Generic;
using System.Threading.Tasks;
using TicketPal.Domain.Models.Request;
using TicketPal.Domain.Models.Response;

namespace TicketPal.Interfaces.Services.Performers
{
    public interface IPerformerService
    {
        Task<OperationResult> AddPerformer(AddPerformerRequest model);
        Task<OperationResult> UpdatePerformer(UpdatePerformerRequest model);
        Task<OperationResult> DeletePerformer(int id);
        Task<List<Performer>> GetPerformers();
        Task<Performer> GetPerformer(int id);
    }
}
