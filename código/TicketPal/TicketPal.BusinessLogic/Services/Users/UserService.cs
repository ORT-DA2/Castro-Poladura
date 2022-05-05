using System.Collections.Generic;
using AutoMapper;
using TicketPal.BusinessLogic.Settings.Api;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Models.Request;
using TicketPal.Domain.Models.Response;
using TicketPal.Interfaces.Repository;
using TicketPal.Interfaces.Services.Users;
using Microsoft.Extensions.Options;
using BC = BCrypt.Net.BCrypt;
using TicketPal.BusinessLogic.Utils.Auth;
using TicketPal.Domain.Entity;
using TicketPal.Domain.Exceptions;
using System.Linq;

namespace TicketPal.BusinessLogic.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository repository;
        private readonly IAppSettings appSettings;
        private readonly IMapper mapper;
        public UserService(
            IUserRepository repository,
            IOptions<IAppSettings> appSettings,
            IMapper mapper
        )
        {
            this.mapper = mapper;
            this.repository = repository;
            this.appSettings = appSettings.Value;
        }
        public OperationResult DeleteUser(int id)
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

        public User GetUser(int id)
        {
            return mapper.Map<User>(repository.Get(id));
        }

        public IEnumerable<User> GetUsers(UserRole role = UserRole.SPECTATOR)
        {
            var users = repository.GetAll(u => u.Role.Equals(role.ToString()));
            return mapper.Map<IEnumerable<UserEntity>, IEnumerable<User>>(users);
        }

        public User Login(AuthenticationRequest model)
        {
            var found = repository.Get(u => u.Email.Equals(model.Email));
            if (found == null || !BC.Verify(model.Password, found.Password))
            {
                return null;
            }
            var token = JwtUtils.GenerateJwtToken(appSettings.JwtSecret, "id", found.Id.ToString());
            var user = mapper.Map<User>(found);

            return user;
        }


        public OperationResult SignUp(SignUpRequest model)
        {
            if (!Values.validRoles.Contains(model.Role))
            {
                return new OperationResult
                {
                    ResultCode = ResultCode.FAIL,
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
                    ResultCode = ResultCode.FAIL,
                    Message = ex.Message
                };
            }
            return new OperationResult
            {
                ResultCode = ResultCode.SUCCESS,
                Message = "User successfully registered"
            };
        }

        public OperationResult UpdateUser(UpdateUserRequest model, UserRole authorization = UserRole.SPECTATOR)
        {
            if (Values.authorizedRolesToUpdate.Contains(authorization.ToString()))
            {
                try
                {
                    if (authorization.Equals(UserRole.SPECTATOR))
                    {
                        repository.Update(
                            new UserEntity
                            {
                                Firstname = model.Firstname,
                                Lastname = model.Lastname,
                                Email = model.Email
                            });
                    }
                    else if (authorization.Equals(UserRole.ADMIN))
                    {
                        if (Values.validRoles.Contains(model.Role))
                        {
                            repository.Update(
                                new UserEntity
                                {
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
                                ResultCode = ResultCode.FAIL,
                                Message = $"Can't validate role: {model.Role}"
                            };
                        }
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

            }
            else
            {
                return new OperationResult
                {
                    ResultCode = ResultCode.FAIL,
                    Message = "Not authorized to update"
                };
            }
            return new OperationResult
            {
                ResultCode = ResultCode.SUCCESS,
                Message = "User updated successfully"
            };
        }
    }
}