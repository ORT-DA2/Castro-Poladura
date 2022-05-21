using System.Collections.Generic;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Models.Request;
using TicketPal.Domain.Models.Response;

namespace TicketPal.Interfaces.Services.Users
{
    public interface IUserService
    {
        User Login(AuthenticationRequest model);
        IEnumerable<User> GetUsers(string role);
        User GetUser(int id);
        User RetrieveUserFromToken(string token);
        OperationResult SignUp(SignUpRequest model);
        OperationResult UpdateUser(UpdateUserRequest model, string authorization);
        OperationResult DeleteUser(int id);
    }
}