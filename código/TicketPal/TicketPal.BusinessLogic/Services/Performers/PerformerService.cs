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
        public IGenericRepository<UserEntity> userRepository;

        public PerformerService(IServiceFactory factory, IMapper mapper)
        {
            this.mapper = mapper;
            this.serviceFactory = factory;
            this.performerRepository = serviceFactory.GetRepository(typeof(PerformerEntity)) as IGenericRepository<PerformerEntity>;
            this.userRepository = serviceFactory.GetRepository(typeof(UserEntity)) as IGenericRepository<UserEntity>;
            this.genreRepository = serviceFactory.GetRepository(typeof(GenreEntity)) as IGenericRepository<GenreEntity>;
        }

        public OperationResult AddPerformer(AddPerformerRequest model)
        {
            try
            {
                GenreEntity genre = genreRepository.Get(model.Genre);
                var members = userRepository.GetAll(c => model.MembersIds.Contains(c.Id));
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

                if (!user.Role.Equals(Constants.ROLE_ARTIST))
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
                    Members = members.ToList(),
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
            var performers = performerRepository.GetAll().ToList();
            return mapper.Map<IEnumerable<PerformerEntity>, IEnumerable<Performer>>(performers);
        }

        public OperationResult UpdatePerformer(UpdatePerformerRequest model)
        {
            try
            {
                var genre = genreRepository.Get(model.GenreId);
                var user = userRepository.Get(model.UserId);
                var artists = performerRepository.GetAll(a => model.ArtistsIds.Contains(a.Id));

                if (!string.IsNullOrEmpty(model.PerformerType) && !Constants.ValidPerformerTypes.Contains(model.PerformerType))
                {
                    return new OperationResult
                    {
                        ResultCode = Constants.CODE_FAIL,
                        Message = "The performer type is not valid"
                    };
                }

                performerRepository.Update(new PerformerEntity
                {
                    Id = model.Id,
                    UserInfo = user,
                    PerformerType = model.PerformerType,
                    Genre = genre,
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
                Message = "Performer updated successfully"
            };
        }
    }
}
