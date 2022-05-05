namespace TicketPal.BusinessLogic.Settings.Api
{
    public class AppSettings : IAppSettings
    {
        public string JwtSecret { get; set; }
    }
}