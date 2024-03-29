﻿using AutoMapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Exceptions;
using TicketPal.Domain.Models.Request;
using TicketPal.Domain.Models.Response;
using TicketPal.Interfaces.Factory;
using TicketPal.Interfaces.Repository;
using TicketPal.Interfaces.Services.Genres;

namespace TicketPal.BusinessLogic.Services.Genres
{
    public class GenreService : IGenreService
    {
        private readonly IServiceFactory serviceFactory;
        private readonly IMapper mapper;
        public IGenericRepository<GenreEntity> genreRepository;

        public GenreService(IServiceFactory factory, IMapper mapper)
        {
            this.mapper = mapper;
            this.serviceFactory = factory;
            this.genreRepository = serviceFactory.GetRepository(typeof(GenreEntity)) as IGenericRepository<GenreEntity>;
        }

        public async Task<OperationResult> AddGenre(AddGenreRequest model)
        {
            try
            {
                GenreEntity found = await genreRepository.Get(g => g.Name == model.Name);

                if (found == null)
                {
                    await genreRepository.Add(new GenreEntity
                    {
                        Name = model.Name
                    });
                }
                else
                {
                    return new OperationResult
                    {
                        ResultCode = Constants.CODE_FAIL,
                        Message = "Genre already exists"
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
                Message = "Genre successfully created"
            };
        }

        public async Task<OperationResult> DeleteGenre(int id)
        {
            try
            {
                await genreRepository.Delete(id);
                return new OperationResult
                {
                    ResultCode = Constants.CODE_SUCCESS,
                    Message = "Genre removed successfully"
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

        public async Task<Genre> GetGenre(int id)
        {
            return mapper.Map<Genre>(await genreRepository.Get(id));
        }

        public async Task<List<Genre>> GetGenres()
        {
            var genres = await genreRepository.GetAll();
            return mapper.Map<List<GenreEntity>, List<Genre>>(genres);
        }

        public OperationResult UpdateGenre(UpdateGenreRequest model)
        {
            try
            {
                genreRepository.Update(new GenreEntity
                {
                    Id = model.Id,
                    Name = model.Name
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
                Message = "Genre updated successfully"
            };
        }
    }
}
