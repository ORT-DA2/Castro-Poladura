namespace TicketPal.Interfaces.Services.Settings
{
    public interface IAppSettings
    {
        string JwtSecret { get; set; }
    }
}