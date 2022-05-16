using System.Collections.Generic;
using AutoMapper;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Models.Request;
using TicketPal.Domain.Models.Response;
using TicketPal.Interfaces.Repository;
using TicketPal.Interfaces.Services.Users;
using Microsoft.Extensions.Options;
using BC = BCrypt.Net.BCrypt;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Exceptions;
using System.Linq;
using TicketPal.Interfaces.Factory;
using TicketPal.Interfaces.Services.Jwt;
using TicketPal.BusinessLogic.Services.Settings;
using System;

namespace TicketPal.BusinessLogic.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<UserEntity> repository;
        private readonly AppSettings appSettings;
        private readonly IMapper mapper;
        private readonly IServiceFactory factory;
        public UserService(
            IServiceFactory factory,
            IOptions<AppSettings> appSettings,
            IMapper mapper
        )
        {
            this.factory = factory;
            this.mapper = mapper;
            this.appSettings = appSettings.Value;

            this.repository = factory.GetRepository(typeof(UserEntity))
                as IGenericRepository<UserEntity>;
        }
        public OperationResult DeleteUser(int id)
        {
            try
            {
                repository.Delete(id);
                return new OperationResult
                {
                    ResultCode = Constants.CODE_SUCCESS,
                    Message = "User removed successfully"
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

        public User GetUser(int id)
        {
            return mapper.Map<User>(repository.Get(id));
        }

        public IEnumerable<User> GetUsers(string role)
        {
            if(string.IsNullOrEmpty(role))
            {
                var users = repository.GetAll();
                return mapper.Map<IEnumerable<UserEntity>, IEnumerable<User>>(users);
            }
            return mapper.Map<IEnumerable<UserEntity>, IEnumerable<User>>(repository.GetAll(u => u.Role.Equals(role)));

        }

        public User Login(AuthenticationRequest model)
        {
            var found = repository.Get(u => u.Email.Equals(model.Email));
            if (found == null || !BC.Verify(model.Password, found.Password))
            {
                return null;
            }
            var jwtService = this.factory.GetService(typeof(IJwtService)) as IJwtService;

            var token = jwtService.GenerateJwtToken(appSettings.JwtSecret, "id", found.Id.ToString());
            var user = mapper.Map<User>(found);
            user.Token = token;

            return user;
        }

        public User RetrieveUserFromToken(string token)
        {
            if(!String.IsNullOrEmpty(token))
            {
                var jwtService = this.factory.GetService(typeof(IJwtService)) as IJwtService;
                var claimToken = jwtService.ClaimTokenValue(this.appSettings.JwtSecret, token, "id");
            
                if(claimToken != null)
                {
                    var accountId = int.Parse(claimToken);
                    return GetUser(accountId);
                }
                else 
                {
                    return null;
                }
            }else
            {
                return null;
            }
        }

        public OperationResult SignUp(SignUpRequest model)
        {
            if (!Constants.ValidRoles.Contains(model.Role))
            {
                return new OperationResult
                {
                    ResultCode = Constants.CODE_FAIL,
                    Message = $"Can't validate role: {model.Role}"
                };
            }

            try
            {
                repository.Add(
                    new UserEntity
                    {
                        Firstname = model.Firstname,
                        Lastname = model.Lastname,
                        Password = BC.HashPassword(model.Password),
                        Role = model.Role,
                        Email = model.Email
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
                Message = "User successfully registered"
            };
        }

        public OperationResult UpdateUser(UpdateUserRequest model, string authorization)
        {
            try
            {
                if (authorization.Equals(Constants.ROLE_SPECTATOR))
                {
                    repository.Update(
                        new UserEntity
                        {
                            Id = model.Id,
                            Firstname = model.Firstname,
                            Lastname = model.Lastname,
                            Email = model.Email
                        });
                }
                else if (authorization.Equals(Constants.ROLE_ADMIN))
                {
                    if (Constants.ValidRoles.Contains(model.Role))
                    {
                        repository.Update(
                            new UserEntity
                            {
                                Id = model.Id,
                                Firstname = model.Firstname,
                                Lastname = model.Lastname,
                                Password = BC.HashPassword(model.Password),
                                Email = model.Email,
                                Role = model.Role
                            });
                    }
                    else
                    {
                        return new OperationResult
                        {
                            ResultCode = Constants.CODE_FAIL,
                            Message = $"Can't validate role: {model.Role}"
                        };
                    }
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
                Message = "User updated successfully"
            };
        }
    }
}