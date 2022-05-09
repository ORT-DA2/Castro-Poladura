namespace TicketPal.Interfaces.Services.Jwt
{
    public interface IJwtService
    {
        string GenerateJwtToken(string secret, string claim, string value);
        string ClaimTokenValue(string secret, string token, string claim);
    }
}