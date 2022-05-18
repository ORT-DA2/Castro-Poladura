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
            CreateMap<User,UserEntity>();
            // Concert
            CreateMap<ConcertEntity,Concert>();
            CreateMap<Concert,ConcertEntity>();
            // Genre
            CreateMap<GenreEntity,Genre>();
            CreateMap<Genre,GenreEntity>();
            // Performer
            CreateMap<Performer,PerformerEntity>();
            CreateMap<PerformerEntity,Performer>();
            // Ticket
            CreateMap<TicketEntity,Ticket>();
            CreateMap<Ticket,TicketEntity>();
        }
    }
}