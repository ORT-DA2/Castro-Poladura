using AutoMapper;
using System;
using System.Collections.Generic;
using TicketPal.BusinessLogic.Utils.TicketCodes;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Exceptions;
using TicketPal.Domain.Models.Request;
using TicketPal.Domain.Models.Response;
using TicketPal.Interfaces.Repository;
using TicketPal.Interfaces.Services.Tickets;

namespace TicketPal.BusinessLogic.Services.Tickets
{
    public class TicketService : ITicketService
    {
        private readonly IServiceFactory serviceFactory;
        private readonly IMapper mapper;
        public IGenericRepository<TicketEntity> ticketRepository;
        public IGenericRepository<ConcertEntity> concertRepository;

        public TicketService(IServiceFactory factory, IMapper mapper)
        {
            this.mapper = mapper;
            this.serviceFactory = factory;
            ticketRepository = serviceFactory.GetRepository<TicketEntity>() as IGenericRepository<TicketEntity>;
            concertRepository = serviceFactory.GetRepository<ConcertEntity>() as IGenericRepository<ConcertEntity>;
        }

        public OperationResult AddTicket(AddTicketRequest model)
        {
            try
            {
                EventEntity newEvent = concertRepository.Get(model.Event);

                var ticket = new TicketCode();//se debe traer la implementacion de ITicketCode;

                UserEntity buyer = new UserEntity()
                {
                    Id = model.User.Id,
                    Firstname = model.User.Firstname,
                    Lastname = model.User.Lastname,
                    Email = model.User.Email,
                    Password = model.User.Password,
                    Role = model.User.Role,
                    ActiveAccount = model.User.ActiveAccount
                };

                ticketRepository.Add(new TicketEntity
                {
                    Buyer = buyer,
                    PurchaseDate = DateTime.Now,
                    Status = TicketStatus.PURCHASED,
                    Code = ticket.GenerateTicketCode(),
                    Event = newEvent
                });
            }
            catch (RepositoryException ex)
            {
                return new OperationResult
                {
                    ResultCode = ResultCode.FAIL,
                    Message = ex.Message
                };
            }
            return new OperationResult
            {
                ResultCode = ResultCode.SUCCESS,
                Message = "Concert successfully created"
            };
        }

        public OperationResult DeleteTicket(int id)
        {
            try
            {
                ticketRepository.Delete(id);
                return new OperationResult
                {
                    ResultCode = ResultCode.SUCCESS,
                    Message = "Ticket removed successfully"
                };
            }
            catch (RepositoryException ex)
            {
                return new OperationResult
                {
                    ResultCode = ResultCode.FAIL,
                    Message = ex.Message
                };
            }
        }

        public Ticket GetTicket(int id)
        {
            return mapper.Map<Ticket>(ticketRepository.Get(id));
        }

        public IEnumerable<Ticket> GetTickets()
        {
            var ticket = ticketRepository.GetAll();
            return mapper.Map<IEnumerable<TicketEntity>, IEnumerable<Ticket>>(ticket);
        }

        public OperationResult UpdateTicket(UpdateTicketRequest model)
        {
            try
            {
                ticketRepository.Update(new TicketEntity
                {
                    Id = model.Id,
                    Code = model.Code,
                    Status = model.Status,
                });
            }
            catch (RepositoryException ex)
            {
                return new OperationResult
                {
                    ResultCode = ResultCode.FAIL,
                    Message = ex.Message
                };
            }

            return new OperationResult
            {
                ResultCode = ResultCode.SUCCESS,
                Message = "Ticket updated successfully"
            };
        }
    }
}
