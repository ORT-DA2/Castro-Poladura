using System;
using AutoMapper;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Models.Response;

namespace TicketPal.BusinessLogic.Mapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            // User
            CreateMap<UserEntity, User>();
            // Concert
            CreateMap<ConcertEntity, Concert>()
            .ForMember(x => x.Date,
                opt => opt.MapFrom(
                    src => ((DateTime)src.Date).ToString("dddd, dd MMMM yyyy HH:mm")));
            // Genre
            CreateMap<GenreEntity, Genre>();
            // Performer
            CreateMap<PerformerEntity, Performer>()
            .ForMember(
                    dest => dest.Members,
                    opt => opt.MapFrom(
                        src => src.Members));
            // Ticket
            CreateMap<TicketEntity, Ticket>();
        }
    }
}