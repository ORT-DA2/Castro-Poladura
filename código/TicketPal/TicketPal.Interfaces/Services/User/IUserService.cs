using System.Collections.Generic;
using TicketPal.Domain.Constants;

namespace TicketPal.Interfaces.Services.User
{
    public interface IUserService
    {
        User Login(AuthenticationRequest model);
        IEnumerable<User> GetUsers(UserRole role = UserRole.SPECTATOR);
        User GetUser(int id);
        OperationResult SignUp(SignInRequest model);
        OperationResult UpdateUser(UpdateUserRequest model, UserRole authorization = UserRole.SPECTATOR);
        OperationResult DeleteUser(int id);
    }
}