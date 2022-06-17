using System.Net;

namespace TicketPal.Domain.Models.Response.Error
{
    public class ForbiddenError : ApiError
    {
        public ForbiddenError()
            : base(403, HttpStatusCode.Forbidden.ToString())
        {
        }

        public ForbiddenError(string message)
            : base(403, HttpStatusCode.Forbidden.ToString(), message)
        {
        }
    }
}