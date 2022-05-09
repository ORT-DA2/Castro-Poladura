using AutoMapper;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Models.Response;

namespace TicketPal.BusinessLogic.Mapper
{
    public class AutoMapping: Profile
    {
        public AutoMapping()
        {
            CreateMap<UserEntity,User>();
            CreateMap<ConcertEntity,Concert>();
            CreateMap<GenreEntity,Genre>();
            CreateMap<PerformerEntity,Performer>();
            CreateMap<TicketEntity,Ticket>();
        }
    }
}