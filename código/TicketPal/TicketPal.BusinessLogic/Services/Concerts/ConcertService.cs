using AutoMapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using TicketPal.BusinessLogic.Settings.Api;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Exceptions;
using TicketPal.Domain.Models.Request;
using TicketPal.Domain.Models.Response;
using TicketPal.Interfaces.Repository;
using TicketPal.Interfaces.Services.Concerts;

namespace TicketPal.BusinessLogic.Services.Concerts
{
    public class ConcertService : IConcertService
    {
        private readonly IConcertRepository repository;
        private readonly IAppSettings appSettings;
        private readonly IMapper mapper;

        public ConcertService(IConcertRepository repository, IOptions<IAppSettings> appSettings, IMapper mapper)
        {
            this.mapper = mapper;
            this.repository = repository;
            this.appSettings = appSettings.Value;
        }

        public OperationResult AddConcert(AddConcertRequest model)
        {
            try
            {
                repository.Add(new ConcertEntity
                    {
                        Artist = model.Artist,
                        AvailableTickets = model.AvailableTickets,
                        CurrencyType = model.CurrencyType,
                        Date = model.Date,
                        EventType = model.EventType,
                        TicketPrice = model.TicketPrice,
                        TourName = model.TourName
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

        public OperationResult DeleteConcert(int id)
        {
            try
            {
                repository.Delete(id);
                return new OperationResult
                {
                    ResultCode = ResultCode.SUCCESS,
                    Message = "User removed successfully"
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

        public bool ExistsConcertByTourName(string tourName)
        {
            throw new NotImplementedException();
        }

        public Concert GetConcert(int id)
        {
            return mapper.Map<Concert>(repository.Get(id));
        }

        public IEnumerable<Concert> GetConcerts()
        {
            var concerts = repository.GetAll();
            return mapper.Map<IEnumerable<ConcertEntity>, IEnumerable<Concert>>(concerts);
        }

        public OperationResult UpdateConcert(UpdateConcertRequest model)
        {
            try
            {
                repository.Update(new ConcertEntity
                {
                    Artist = model.Artist,
                    AvailableTickets = model.AvailableTickets,
                    CurrencyType = model.CurrencyType,
                    Date = model.Date,
                    EventType = model.EventType,
                    TicketPrice = model.TicketPrice,
                    TourName = model.TourName
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
                Message = "Concert updated successfully"
            };
        }
    }
}
