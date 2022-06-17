
namespace TicketPal.BusinessLogic.Services.Settings
{
    public class AppSettings
    {
        public AppSettings()
        {
            JwtSecret = "dh7283g6rt7fd23d9123fb10wdbiwidh1d90734178gdhsg";
        }
        public string JwtSecret { get; set; }
    }
}