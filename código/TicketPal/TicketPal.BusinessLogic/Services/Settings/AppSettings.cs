using TicketPal.Interfaces.Services.Settings;

namespace TicketPal.BusinessLogic.Services.Settings
{
    public class AppSettings : IAppSettings
    {
        public string JwtSecret { get; set; }
    }
}