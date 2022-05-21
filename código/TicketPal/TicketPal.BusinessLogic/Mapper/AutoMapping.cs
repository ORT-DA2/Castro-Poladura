using AutoMapper;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Models.Response;

namespace TicketPal.BusinessLogic.Mapper
{
    public class AutoMapping: Profile
    {
        public AutoMapping()
        {
            // User
            CreateMap<UserEntity,User>();
            // Concert
            CreateMap<ConcertEntity,Concert>();
            // Genre
            CreateMap<GenreEntity,Genre>();
            // Performer
            CreateMap<PerformerEntity,Performer>()
            .ForMember(
                    dest => dest.Concerts,
                    opt => opt.MapFrom(
                        src => src.Concerts));
            // Ticket
            CreateMap<TicketEntity,Ticket>();
        }
    }
}