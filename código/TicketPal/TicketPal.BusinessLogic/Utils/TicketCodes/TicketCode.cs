using System;
using TicketPal.Interfaces.Utils.TicketCodes;

namespace TicketPal.BusinessLogic.Utils.TicketCodes
{
    public class TicketCode : ITicketCode
    {
        public string GenerateTicketCode()
        {
            string UUID = Guid.NewGuid().ToString();
            return UUID;
        }
    }
}
