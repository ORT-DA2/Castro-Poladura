using System.Collections.Generic;
using System.Threading.Tasks;
using TicketPal.Domain.Models.Request;
using TicketPal.Domain.Models.Response;

namespace TicketPal.Interfaces.Services.Concerts
{
    public interface IConcertService
    {
        Task<OperationResult> AddConcert(AddConcertRequest model);
        Task<OperationResult> UpdateConcert(UpdateConcertRequest model);
        Task<OperationResult> DeleteConcert(int id);
        Task<List<Concert>> GetConcerts(string type, bool newest, string startDate, string endDate, string artistName);
        Task<Concert> GetConcert(int id);
    }
}
