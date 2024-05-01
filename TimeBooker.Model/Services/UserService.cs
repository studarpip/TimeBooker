using System.Threading.Tasks;
using TimeBooker.Model.Entities;
using TimeBooker.Model.Entities.Requests;
using TimeBooker.Model.Repositories;

namespace TimeBooker.Model.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<int> CreateUserAsync(User newUser) => await _userRepository.InsertAsync(newUser);

        public async Task<bool> EmailExistsAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return user is not null;
        }

        public async Task<User> LoginAsync(LoginRequest request) => await _userRepository.LoginAsync(request);
    }
}
