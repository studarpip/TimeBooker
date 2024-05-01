using System.Threading.Tasks;
using TimeBooker.Model.Entities;
using TimeBooker.Model.Entities.Requests;

namespace TimeBooker.Model.Services
{
    public interface IUserService
    {
        Task<int> CreateUserAsync(User newUser);
        Task<bool> EmailExistsAsync(string email);
        Task<User> LoginAsync(LoginRequest request);
    }
}
