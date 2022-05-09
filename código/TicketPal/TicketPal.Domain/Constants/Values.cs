namespace TicketPal.Domain.Constants
{
    public static class Values
    {
        public static string[] validRoles = new string[]
        {
            UserRole.ADMIN.ToString(),
            UserRole.SELLER.ToString(),
            UserRole.SPECTATOR.ToString(),
            UserRole.SUPERVISOR.ToString()
        };
    }
}