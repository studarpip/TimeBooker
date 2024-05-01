using System.Threading.Tasks;
using TimeBooker.Model.Entities;
using TimeBooker.Model.Entities.Requests;

namespace TimeBooker.Model.Repositories
{
    public interface IUserRepository
    {
        public Task<User> GetByEmailAsync(string email);
        public Task<int> InsertAsync(User user);
        public Task<User> LoginAsync(LoginRequest request);
    }
}
