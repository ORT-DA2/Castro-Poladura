using AutoMapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using TicketPal.BusinessLogic.Settings.Api;
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

        public PerformerService(IServiceFactory factory, IMapper mapper)
        {
            this.mapper = mapper;
            this.serviceFactory = factory;
            this.performerRepository = serviceFactory.GetRepository(typeof(PerformerEntity)) as IGenericRepository<PerformerEntity>;
            this.genreRepository = serviceFactory.GetRepository(typeof(GenreEntity)) as IGenericRepository<GenreEntity>;
        }

        public OperationResult AddPerformer(AddPerformerRequest model)
        {
            try
            {
                GenreEntity genre = genreRepository.Get(model.Genre);
                PerformerEntity found = performerRepository.Get(p => p.Name == model.Name);

                if (found == null)
                {
                    if (genre != null)
                    {
                        performerRepository.Add(new PerformerEntity
                        {
                            Name = model.Name,
                            Artists = (model.Artists == null ? "" : model.Artists),
                            Genre = genre,
                            PerformerType = model.PerformerType,
                            StartYear = model.StartYear

                        });
                    }
                    else
                    {
                        return new OperationResult
                        {
                            ResultCode = ResultCode.FAIL,
                            Message = "Genre doesn't exists"
                        };
                    }
                }
                else
                {
                    return new OperationResult
                    {
                        ResultCode = ResultCode.FAIL,
                        Message = "Performer already exists"
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
                    ResultCode = ResultCode.SUCCESS,
                    Message = "Performer removed successfully"
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
                    Name = model.Name,
                    Artists = model.Artists,
                    Genre = genre,
                    PerformerType = model.PerformerType,
                    StartYear = model.StartYear
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
