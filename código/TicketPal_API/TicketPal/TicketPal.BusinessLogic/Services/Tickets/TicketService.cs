﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketPal.BusinessLogic.Utils.TicketCodes;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Exceptions;
using TicketPal.Domain.Models.Request;
using TicketPal.Domain.Models.Response;
using TicketPal.Interfaces.Factory;
using TicketPal.Interfaces.Repository;
using TicketPal.Interfaces.Services.Tickets;
using TicketPal.Interfaces.Services.Users;
using TicketPal.Interfaces.Utils.TicketCodes;

namespace TicketPal.BusinessLogic.Services.Tickets
{
    public class TicketService : ITicketService
    {
        private readonly IServiceFactory serviceFactory;
        private readonly IMapper mapper;
        private IGenericRepository<TicketEntity> ticketRepository;
        private IGenericRepository<ConcertEntity> concertRepository;
        private IGenericRepository<UserEntity> userRepository;

        public TicketService(IServiceFactory factory, IMapper mapper)
        {
            this.mapper = mapper;
            this.serviceFactory = factory;
            ticketRepository = serviceFactory.GetRepository(typeof(TicketEntity)) as IGenericRepository<TicketEntity>;
            concertRepository = serviceFactory.GetRepository(typeof(ConcertEntity)) as IGenericRepository<ConcertEntity>;
            userRepository = serviceFactory.GetRepository(typeof(UserEntity)) as IGenericRepository<UserEntity>;
        }

        public async Task<OperationResult> AddTicket(AddTicketRequest model)
        {
            try
            {
                var newEvent = await concertRepository.Get(model.EventId);
                var ticketCode = serviceFactory.GetService(typeof(ITicketCode)) as ITicketCode;

                if (newEvent != null)
                {
                    if (model.UserLogged)
                    {
                        var retrievedUser = await userRepository.Get(model.LoggedUserId);

                        ticketRepository.Add(new TicketEntity
                        {
                            Buyer = retrievedUser,
                            PurchaseDate = DateTime.Now,
                            Status = Constants.TICKET_PURCHASED_STATUS,
                            Code = ticketCode.GenerateTicketCode(),
                            Event = newEvent
                        });
                    }
                    else
                    {
                        var buyer = new UserEntity();
                        buyer.Firstname = model.NewUser.FirstName;
                        buyer.Lastname = model.NewUser.LastName;
                        buyer.Email = model.NewUser.Email;
                        buyer.ActiveAccount = false;

                        ticketRepository.Add(new TicketEntity
                        {
                            Buyer = buyer,
                            PurchaseDate = DateTime.Now,
                            Status = Constants.TICKET_PURCHASED_STATUS,
                            Code = ticketCode.GenerateTicketCode(),
                            Event = newEvent
                        });
                    }

                }
                else
                {
                    return new OperationResult
                    {
                        ResultCode = Constants.CODE_FAIL,
                        Message = "Event doesn't exists"
                    };
                }
            }
            catch (RepositoryException ex)
            {
                return new OperationResult
                {
                    ResultCode = Constants.CODE_FAIL,
                    Message = ex.Message
                };
            }
            return new OperationResult
            {
                ResultCode = Constants.CODE_SUCCESS,
                Message = "Ticket successfully purchased"
            };
        }

        public OperationResult DeleteTicket(int id)
        {
            try
            {
                ticketRepository.Delete(id);
                return new OperationResult
                {
                    ResultCode = Constants.CODE_SUCCESS,
                    Message = "Ticket removed successfully"
                };
            }
            catch (RepositoryException ex)
            {
                return new OperationResult
                {
                    ResultCode = Constants.CODE_FAIL,
                    Message = ex.Message
                };
            }
        }

        public async Task<Ticket> GetTicket(int id)
        {
            return mapper.Map<Ticket>(await ticketRepository.Get(id));
        }

        public async Task<List<Ticket>> GetUserTickets(int userId)
        {
            var ticket = await ticketRepository.GetAll(t => t.Buyer.Id == userId);
            return mapper.Map<List<TicketEntity>, List<Ticket>>(ticket);
        }

        public async Task<List<Ticket>> GetTickets()
        {
            var ticket = await ticketRepository.GetAll();
            return mapper.Map<List<TicketEntity>, List<Ticket>>(ticket);
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
                    ResultCode = Constants.CODE_FAIL,
                    Message = ex.Message
                };
            }

            return new OperationResult
            {
                ResultCode = Constants.CODE_SUCCESS,
                Message = "Ticket updated successfully"
            };
        }
    }
}
