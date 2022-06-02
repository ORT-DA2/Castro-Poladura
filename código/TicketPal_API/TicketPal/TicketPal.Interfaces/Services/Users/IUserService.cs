using System.Collections.Generic;
using System.Threading.Tasks;
using TicketPal.Domain.Constants;
using TicketPal.Domain.Models.Request;
using TicketPal.Domain.Models.Response;

namespace TicketPal.Interfaces.Services.Users
{
    public interface IUserService
    {
        Task<User> Login(AuthenticationRequest model);
        Task<List<User>> GetUsers(string role);
        Task<User> GetUser(int id);
        Task<User> RetrieveUserFromToken(string token);
        OperationResult SignUp(SignUpRequest model);
        OperationResult UpdateUser(UpdateUserRequest model, string authorization);
        OperationResult DeleteUser(int id);
    }
}