using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Server.IIS.Core;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Exceptions;
using TicketPal.Domain.Models.Param;
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

        public async Task<OperationResult> AddPerformer(AddPerformerRequest model)
        {
            try
            {
                GenreEntity genre = await genreRepository.Get(model.Genre);
                var members = await performerRepository.GetAll(c => model.MembersIds.Contains(c.Id));
                var user = await userRepository.Get(model.UserId);

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
                await performerRepository.Add(new PerformerEntity
                {
                    UserInfo = user,
                    Members = members,
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

        public async Task<OperationResult> DeletePerformer(int id)
        {
            try
            {
                await performerRepository.Delete(id);
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

        public async Task<Performer> GetPerformer(int id)
        {
            return mapper.Map<Performer>(await performerRepository.Get(id));
        }

        public async Task<List<Performer>> GetPerformers(PerformerSearchParam param)
        {
            var performers = new List<PerformerEntity>();
            var hasName = !String.IsNullOrEmpty(param.PerformerName);

            if (!hasName)
            {
                performers = await performerRepository.GetAll();
            }
            else
            {
                performers = await performerRepository.GetAll();
                performers = performers.FindAll(p =>
                   p.UserInfo.Firstname.ToLower() + " " + p.UserInfo.Lastname.ToLower() == param.PerformerName.ToLower());
            }
            
            return mapper.Map<List<PerformerEntity>, List<Performer>>(performers);
        }

        public async Task<OperationResult> UpdatePerformer(UpdatePerformerRequest model)
        {
            try
            {
                var genre = await genreRepository.Get(model.GenreId);
                var user = await userRepository.Get(model.UserId);
                var existingPerformers = user.Performer.Members;
                var newArtists = await performerRepository.GetAll(a => model.ArtistsIds.Contains(a.Id));
                var resultingArtists = existingPerformers.Union(newArtists);

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
                    StartYear = model.StartYear,
                    Members = resultingArtists.ToList()
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
