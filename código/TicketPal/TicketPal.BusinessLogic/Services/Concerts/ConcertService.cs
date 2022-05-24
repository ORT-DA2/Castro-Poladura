using AutoMapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
                var found = concertRepository.Get(c => c.TourName == model.TourName && c.Date == model.Date);
                var artists = performerRepository.GetAll(a => model.ArtistsIds.Contains(a.UserInfo.Id));
                if (found == null)
                {
                    concertRepository.Add(new ConcertEntity
                    {
                        Artists = artists.ToList(),
                        AvailableTickets = model.AvailableTickets,
                        CurrencyType = model.CurrencyType,
                        Date = model.Date,
                        EventType = model.EventType,
                        TicketPrice = model.TicketPrice,
                        TourName = model.TourName,
                        Location = model.Location,
                        Country = model.Country,
                        Address = model.Address
                    });
                }
                else
                {
                    return new OperationResult
                    {
                        ResultCode = Constants.CODE_FAIL,
                        Message = "Concert already exists"
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
                    ResultCode = Constants.CODE_SUCCESS,
                    Message = "Concert removed successfully"
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

        public Concert GetConcert(int id)
        {
            return mapper.Map<Concert>(concertRepository.Get(id));
        }

        public IEnumerable<Concert> GetConcerts(string type, bool newest, string startDate, string endDate, string artistName)
        {
            IEnumerable<ConcertEntity> concerts = new List<ConcertEntity>();

            if (String.IsNullOrEmpty(startDate)
                && String.IsNullOrEmpty(endDate)
                && String.IsNullOrEmpty(artistName)
                )
            {
                concerts = concertRepository.GetAll(
                    c => c.EventType.Equals(type)
                );
            }
            else if (!String.IsNullOrEmpty(startDate)
            && String.IsNullOrEmpty(endDate)
            && String.IsNullOrEmpty(artistName)
            )
            {
                var dtStart = DateTime.ParseExact(startDate,
                       "dd/M/yyyy hh:mm",
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None);

                concerts = concertRepository.GetAll(
                    c => c.EventType.Equals(type)
                        && c.Date >= dtStart
                );
            }
            else if (String.IsNullOrEmpty(startDate)
            && !String.IsNullOrEmpty(endDate)
            && String.IsNullOrEmpty(artistName)
            )
            {
                var dtEnd = DateTime.ParseExact(endDate,
                       "dd/M/yyyy hh:mm",
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None);

                concerts = concertRepository.GetAll(
                    c => c.EventType.Equals(type)
                        && c.Date >= dtEnd
                );
            }
            else if (String.IsNullOrEmpty(startDate)
            && String.IsNullOrEmpty(endDate)
            && !String.IsNullOrEmpty(artistName)
            )
            {
                concerts = concertRepository.GetAll(
                    c => c.Artists
                        .Any(a => a.UserInfo.Firstname.Equals(artistName) || a.UserInfo.Lastname.Equals(artistName))
                );
            }
            else if (!String.IsNullOrEmpty(startDate)
            && !String.IsNullOrEmpty(endDate)
            && String.IsNullOrEmpty(artistName))
            {
                var dtStart = DateTime.ParseExact(startDate,
                       "dd/M/yyyy hh:mm",
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None);
                var dtEnd = DateTime.ParseExact(endDate,
                       "dd/M/yyyy hh:mm",
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None);

                concerts = concertRepository.GetAll(
                   c => c.EventType.Equals(type)
                       && (c.Date >= dtStart && c.Date <= dtEnd)
               );
            }
            else
            {
                var dtStart = DateTime.ParseExact(startDate,
                       "dd/M/yyyy hh:mm",
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None);
                var dtEnd = DateTime.ParseExact(endDate,
                       "dd/M/yyyy hh:mm",
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None);

                concerts = concertRepository.GetAll(
                    c => c.EventType.Equals(type)
                        && (c.Date >= dtStart && c.Date <= dtEnd)
                        && c.Artists.Any(a => a.UserInfo.Firstname.Equals(artistName) || a.UserInfo.Lastname.Equals(artistName))
                );
            }
            if (!newest)
            {
                return mapper.Map<IEnumerable<ConcertEntity>, IEnumerable<Concert>>(
                    concerts.OrderBy(c => c.Date));
            }
            else
            {
                return mapper.Map<IEnumerable<ConcertEntity>, IEnumerable<Concert>>(
                    concerts.OrderByDescending(c => c.Date));
            }

        }

        public OperationResult UpdateConcert(UpdateConcertRequest model)
        {
            try
            {

                concertRepository.Update(new ConcertEntity
                {
                    Id = model.Id,
                    CurrencyType = model.CurrencyType,
                    Date = model.Date,
                    EventType = model.EventType,
                    TicketPrice = model.TicketPrice,
                    TourName = model.TourName,
                    Location = model.Location,
                    Address = model.Address,
                    Country = model.Country
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
                Message = "Concert updated successfully"
            };
        }
    }
}
