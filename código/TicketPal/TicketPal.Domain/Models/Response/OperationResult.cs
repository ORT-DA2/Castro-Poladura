using TicketPal.Domain.Constants;

namespace TicketPal.Domain.Models.Response
{
    public class OperationResult
    {
        public string Message { get; set; }
        public ResultCode ResultCode { get; set; }
    }
}