namespace TicketPal.Domain.Constants
{
    public enum TicketStatus
    {
        PURCHASED = 1,
        USED = 2
    }
    public enum PerformerType
    {
        SOLO_ARTIST = 1,
        BAND = 2
    }

    public enum EventType
    {
        CONCERT = 1
    }

    public enum CurrencyType
    {
        UYU = 1,
        USD = 2
    }

    public enum ResultCode
    {
        SUCCESS = 0,
        FAIL = 1
    }

    public enum UserRole
    {
        SPECTATOR = 0,
        SELLER = 1,
        SUPERVISOR = 2,
        ADMIN = 3
    }
}