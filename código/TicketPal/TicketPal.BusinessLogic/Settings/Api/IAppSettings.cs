namespace TicketPal.BusinessLogic.Settings.Api
{
    public interface IAppSettings
    {
        string JwtSecret { get; set; }
    }
}