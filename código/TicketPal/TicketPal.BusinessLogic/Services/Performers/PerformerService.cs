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
using TicketPal.Interfaces.Repository;
using TicketPal.Interfaces.Services.Performers;

namespace TicketPal.BusinessLogic.Services.Performers
{
    public class PerformerService : IPerformerService
    {
        private readonly IPerformerRepository repository;
        private readonly IAppSettings appSettings;
        private readonly IMapper mapper;

        public PerformerService(IPerformerRepository repository, IOptions<IAppSettings> appSettings, IMapper mapper)
        {
            this.mapper = mapper;
            this.repository = repository;
            this.appSettings = appSettings.Value;
        }

        public OperationResult AddPerformer(AddPerformerRequest model)
        {
            try
            {
                repository.Add(new PerformerEntity
                {
                    Name = model.Name,
                    Artists = (model.Artists == null ? "" : model.Artists),
                    Genre = model.Genre,
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
                Message = "Concert successfully created"
            };
        }

        public OperationResult DeletePerformer(int id)
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

        public bool ExistsPerformerByName(string name)
        {
            throw new NotImplementedException();
        }

        public Performer GetPerformer(int id)
        {
            return mapper.Map<Performer>(repository.Get(id));
        }

        public IEnumerable<Performer> GetPerformers()
        {
            var performers = repository.GetAll();
            return mapper.Map<IEnumerable<PerformerEntity>, IEnumerable<Performer>>(performers);
        }

        public OperationResult UpdatePerformer(UpdatePerformerRequest model)
        {
            try
            {
                repository.Update(new PerformerEntity
                {
                    Name = model.Name,
                    Artists = model.Artists,
                    Genre = model.Genre,
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
