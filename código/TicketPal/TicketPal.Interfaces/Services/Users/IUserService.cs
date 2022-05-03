using System.Collections.Generic;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Models.Request;
using TicketPal.Domain.Models.Response;

namespace TicketPal.Interfaces.Services.Users
{
    public interface IUserService
    {
        User Login(AuthenticationRequest model);
        IEnumerable<User> GetUsers(UserRole role = UserRole.SPECTATOR);
        User GetUser(int id);
        OperationResult SignUp(SignUpRequest model);
        OperationResult UpdateUser(UpdateUserRequest model, UserRole authorization = UserRole.SPECTATOR);
        OperationResult DeleteUser(int id);
    }
}