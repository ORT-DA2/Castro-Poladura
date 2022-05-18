using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Exceptions;
using TicketPal.Domain.Models.Request;
using TicketPal.Domain.Models.Response;
using TicketPal.Interfaces.Factory;
using TicketPal.Interfaces.Repository;
using TicketPal.Interfaces.Services.Performers;

namespace TicketPal.BusinessLogic.Services.Performers
{
    public class PerformerService : IPerformerService
    {
        private readonly IServiceFactory serviceFactory;
        private readonly IMapper mapper;
        public IGenericRepository<PerformerEntity> performerRepository;
        public IGenericRepository<GenreEntity> genreRepository;
        public IGenericRepository<ConcertEntity> concertRepository;
        public IGenericRepository<UserEntity> userRepository;

        public PerformerService(IServiceFactory factory, IMapper mapper)
        {
            this.mapper = mapper;
            this.serviceFactory = factory;
            this.performerRepository = serviceFactory.GetRepository(typeof(PerformerEntity)) as IGenericRepository<PerformerEntity>;
            this.concertRepository = serviceFactory.GetRepository(typeof(ConcertEntity)) as IGenericRepository<ConcertEntity>;
            this.userRepository = serviceFactory.GetRepository(typeof(UserEntity)) as IGenericRepository<UserEntity>;
            this.genreRepository = serviceFactory.GetRepository(typeof(GenreEntity)) as IGenericRepository<GenreEntity>;
        }

        public OperationResult AddPerformer(AddPerformerRequest model)
        {
            try
            {
                GenreEntity genre = genreRepository.Get(model.Genre);
                var concerts = concertRepository.GetAll(c => model.ConcertIds.Contains(c.Id));
                var user = userRepository.Get(model.UserId);

                if (genre == null)
                {
                    return new OperationResult
                    {
                        ResultCode = Constants.CODE_FAIL,
                        Message = "Genre doesn't exists"
                    };
                }
                if (user == null)
                {
                    return new OperationResult
                    {
                        ResultCode = Constants.CODE_FAIL,
                        Message = "User doesn't exists"
                    };
                }
                
                if(!user.Role.Equals(Constants.ROLE_ARTIST)) 
                {
                        return new OperationResult
                        {
                            ResultCode = Constants.CODE_FAIL,
                            Message = "The associated user account is not from a performer"
                        };
                }
                performerRepository.Add(new PerformerEntity
                {
                    UserInfo = user,
                    Concerts = concerts,
                    Genre = genre,
                    PerformerType = model.PerformerType,
                    StartYear = model.StartYear

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
                Message = "Performer successfully created"
            };
        }

        public OperationResult DeletePerformer(int id)
        {
            try
            {
                performerRepository.Delete(id);
                return new OperationResult
                {
                    ResultCode = Constants.CODE_SUCCESS,
                    Message = "Performer removed successfully"
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

        public Performer GetPerformer(int id)
        {
            return mapper.Map<Performer>(performerRepository.Get(id));
        }

        public IEnumerable<Performer> GetPerformers()
        {
            var performers = performerRepository.GetAll();
            return mapper.Map<IEnumerable<PerformerEntity>, IEnumerable<Performer>>(performers);
        }

        public OperationResult UpdatePerformer(UpdatePerformerRequest model)
        {
            try
            {
                GenreEntity genre = genreRepository.Get(model.Genre);

                performerRepository.Update(new PerformerEntity
                {
                    Id = model.Id,
                    UserInfo = mapper.Map<UserEntity>(model.UserInfo),
                    Concerts = mapper.Map<IEnumerable<ConcertEntity>>(model.Artists),
                    Genre = genre,
                    PerformerType = model.PerformerType,
                    StartYear = model.StartYear
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
