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
using TicketPal.Interfaces.Services.Genres;

namespace TicketPal.BusinessLogic.Services.Genres
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository repository;
        private readonly IAppSettings appSettings;
        private readonly IMapper mapper;

        public GenreService(IGenreRepository repository, IOptions<IAppSettings> appSettings, IMapper mapper)
        {
            this.mapper = mapper;
            this.repository = repository;
            this.appSettings = appSettings.Value;
        }

        public OperationResult AddGenre(AddGenreRequest model)
        {
            try
            {
                repository.Add(new GenreEntity
                {
                    GenreName = model.GenreName
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

        public OperationResult DeleteGenre(int id)
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

        public bool ExistsGenreByName(string name)
        {
            throw new NotImplementedException();
        }

        public Genre GetGenre(int id)
        {
            return mapper.Map<Genre>(repository.Get(id));
        }

        public IEnumerable<Genre> GetGenres()
        {
            var genres = repository.GetAll();
            return mapper.Map<IEnumerable<GenreEntity>, IEnumerable<Genre>>(genres);
        }

        public OperationResult UpdateGenre(UpdateGenreRequest model)
        {
            try
            {
                repository.Update(new GenreEntity
                {
                    GenreName = model.GenreName
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
