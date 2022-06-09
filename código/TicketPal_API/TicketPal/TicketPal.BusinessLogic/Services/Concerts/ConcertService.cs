using AutoMapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Exceptions;
using TicketPal.Domain.Models.Param;
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

        public async Task<OperationResult> AddConcert(AddConcertRequest model)
        {
            try
            {
                var found = await concertRepository.Get(c => c.TourName == model.TourName && c.Date == model.Date);
                var artists = await performerRepository.GetAll(a => model.ArtistsIds.Contains(a.UserInfo.Id));

                if (found == null)
                {
                    await concertRepository.Add(new ConcertEntity
                    {
                        Artists = artists,
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

        public async Task<OperationResult> DeleteConcert(int id)
        {
            try
            {
                await concertRepository.Delete(id);
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

        public async Task<Concert> GetConcert(int id)
        {
            var concert = await concertRepository.Get(id);
            return mapper.Map<Concert>(concert);
        }

        public async Task<List<Concert>> GetConcerts(ConcertSearchParam param)
        {
            List<ConcertEntity> concerts = new List<ConcertEntity>();
            var dtStart = new DateTime();
            var dtEnd = new DateTime();
            var hasStartDate = !String.IsNullOrEmpty(param.StartDate);
            var hasEndDate = !String.IsNullOrEmpty(param.EndDate);
            var hasArtist = !String.IsNullOrEmpty(param.ArtistName);
            var hasTourName = !String.IsNullOrEmpty(param.TourName);

            if (!String.IsNullOrEmpty(param.StartDate))
            {
                dtStart = DateTime.ParseExact(param.StartDate,
                       "dd/M/yyyy HH:mm",
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None);
            }
            if (!String.IsNullOrEmpty(param.EndDate))
            {
                dtEnd = DateTime.ParseExact(param.EndDate,
                       "dd/M/yyyy HH:mm",
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None);
            }

            concerts = await concertRepository.GetAll(c => c.EventType.Equals(param.Type));

            if (hasArtist)
            {
                concerts = concerts.FindAll(c =>
                    c.EventType.Equals(param.Type)
                && c.Artists.Any(a =>
                    a.UserInfo.Firstname.ToLower()
                        .Equals(param.ArtistName) ||
                    a.UserInfo.Lastname.ToLower()
                        .Equals(param.ArtistName))
                );
            }

            if (hasTourName)
            {
                concerts = concerts.FindAll(c =>
                    c.TourName.ToLower().Equals(param.TourName.ToLower())
                );
            }

            if (hasEndDate && hasStartDate)
            {
                concerts = concerts.FindAll(c =>
                    c.Date >= dtStart && c.Date <= dtEnd
                );
            }
            else
            {
                if (hasEndDate)
                {
                    concerts = concerts.FindAll(c =>
                        c.Date.Day == dtEnd.Day
                        && c.Date.Month == dtEnd.Month
                        && c.Date.Year == dtEnd.Year
                    );
                }
                else if (hasStartDate)
                {
                    concerts = concerts.FindAll(c =>
                        c.Date.Day == dtStart.Day
                        && c.Date.Month == dtStart.Month
                        && c.Date.Year == dtStart.Year
                    );
                }
            }

            if (!param.Newest)
            {
                return mapper.Map<List<ConcertEntity>, List<Concert>>(
                    concerts.OrderBy(c => c.Date).ToList());
            }
            else
            {
                return mapper.Map<List<ConcertEntity>, List<Concert>>(
                    concerts.OrderByDescending(c => c.Date).ToList());
            }
        }

        public async Task<OperationResult> UpdateConcert(UpdateConcertRequest model)
        {
            try
            {
                var existingPerformers = (await concertRepository.Get(model.Id)).Artists;
                var newPerformers = await performerRepository.GetAll(a => model.ArtistsIds.Contains(a.Id));
                var resultingArtists = existingPerformers.Union(newPerformers);

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
                    Country = model.Country,
                    Artists = resultingArtists.ToList()
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
