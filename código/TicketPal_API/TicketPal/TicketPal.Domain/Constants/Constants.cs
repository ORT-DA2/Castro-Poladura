namespace TicketPal.Domain.Constants
{
    public static class Constants
    {
        // Roles
        public static string[] ValidRoles = new string[]
        {
            ROLE_SPECTATOR,
            ROLE_SELLER,
            ROLES_SUPERVISOR,
            ROLE_ADMIN,
            ROLE_ARTIST
        };
        // Performer Types
        public static string[] ValidPerformerTypes = new string[]
        {
            PERFORMER_TYPE_BAND,
            PERFORMER_TYPE_SOLO_ARTIST
        };
        
        public const string ROLE_SPECTATOR = "SPECTATOR";
        public const string ROLE_SELLER = "SELLER";
        public const string ROLES_SUPERVISOR = "SUPERVISOR";
        public const string ROLE_ADMIN = "ADMIN";
        public const string ROLE_ARTIST = "ARTIST";

        // Tickets
        public const string TICKET_PURCHASED_STATUS = "PURCHASED";
        public const string TICKET_USED_STATUS = "USED";

        // Events
        public const string EVENT_CONCERT_TYPE = "CONCERT";

        // Currency Type
        public const string CURRENCY_URUGUAYAN_PESO = "UYU";
        public const string CURRENCY_US_DOLLARS = "USD";

        // Result Codes
        public const string CODE_SUCCESS = "SUCCESS";
        public const string CODE_FAIL = "FAIL";

        // Performer types
        public const string PERFORMER_TYPE_BAND = "BAND";
        public const string PERFORMER_TYPE_SOLO_ARTIST = "SOLO_ARTIST";

        // Export-Import
        public const string EXPORT = "EXPORT";
        public const string EXPORT_PATH = "../TicketPal/files/export/export.txt";
        public const string IMPORT_PATH = "../TicketPal/files/import/import.txt";

    }
}