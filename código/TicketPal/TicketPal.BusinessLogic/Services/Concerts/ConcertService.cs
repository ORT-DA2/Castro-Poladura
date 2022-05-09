using AutoMapper;
using System.Collections.Generic;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Exceptions;
using TicketPal.Domain.Models.Request;
using TicketPal.Domain.Models.Response;
using TicketPal.Interfaces.Factory;
using TicketPal.Interfaces.Repository;
using TicketPal.Interfaces.Services.Concerts;

namespace TicketPal.BusinessLogic.Services.Concerts
{
    public class ConcertService : IConcertService
    {
        private readonly IServiceFactory serviceFactory;
        private readonly IMapper mapper;
        public IGenericRepository<ConcertEntity> concertRepository;
        public IGenericRepository<PerformerEntity> performerRepository;

        public ConcertService(IServiceFactory factory, IMapper mapper)
        {
            this.mapper = mapper;
            this.serviceFactory = factory;
            this.concertRepository = serviceFactory.GetRepository(typeof(ConcertEntity)) as IGenericRepository<ConcertEntity>;
            this.performerRepository = serviceFactory.GetRepository(typeof(PerformerEntity)) as IGenericRepository<PerformerEntity>;
        }

        public OperationResult AddConcert(AddConcertRequest model)
        {
            try
            {
                PerformerEntity artist = performerRepository.Get(model.Artist);
                ConcertEntity found = concertRepository.Get(c => c.TourName == model.TourName && c.Artist.Name == artist.Name && c.Date == model.Date);

                if (found == null)
                {
                    if (artist != null)
                    {
                        concertRepository.Add(new ConcertEntity
                        {
                            Artist = artist,
                            AvailableTickets = model.AvailableTickets,
                            CurrencyType = model.CurrencyType,
                            Date = model.Date,
                            EventType = model.EventType,
                            TicketPrice = model.TicketPrice,
                            TourName = model.TourName
                        });
                    }
                    else
                    {
                        return new OperationResult
                        {
                            ResultCode = ResultCode.FAIL,
                            Message = "Artist doesn't exists"
                        };
                    }
                    
                }
                else
                {
                    return new OperationResult
                    {
                        ResultCode = ResultCode.FAIL,
                        Message = "Concert already exists"
                    };
                }
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
                concertRepository.Delete(id);
                return new OperationResult
                {
                    ResultCode = ResultCode.SUCCESS,
                    Message = "Concert removed successfully"
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

        public Concert GetConcert(int id)
        {
            return mapper.Map<Concert>(concertRepository.Get(id));
        }

        public IEnumerable<Concert> GetConcerts()
        {
            var concerts = concertRepository.GetAll();
            return mapper.Map<IEnumerable<ConcertEntity>, IEnumerable<Concert>>(concerts);
        }

        public OperationResult UpdateConcert(UpdateConcertRequest model)
        {
            try
            {
                PerformerEntity artist = performerRepository.Get(model.Artist);

                concertRepository.Update(new ConcertEntity
                {
                    Id = model.Id,
                    Artist = artist,
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
