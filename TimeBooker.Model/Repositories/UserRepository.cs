using System.Threading.Tasks;
using TimeBooker.Model.Entities;
using TimeBooker.Model.Entities.Requests;

namespace TimeBooker.Model.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IRepository _repository;

        public UserRepository(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var sql = @"SELECT * FROM users WHERE Email = @Email";

            return await _repository.QueryAsync<User, dynamic>(sql, new { Email = email });
        }

        public async Task<int> InsertAsync(User user)
        {
            var sql = @"
            INSERT INTO users (`Name`, `Password`, `Email`)
            VALUES (@Name, @Password, @Email);
            SELECT LAST_INSERT_ID();";

            return await _repository.QueryAsync<int, dynamic>(sql, new
            {
                user.Name,
                user.Password,
                user.Email
            });
        }

        public async Task<User> LoginAsync(LoginRequest request)
        {
            var sql = @"SELECT * FROM users WHERE Email = @Email AND Password = @Password";
            return await _repository.QueryAsync<User, dynamic>(sql, new { request.Email, request.Password });
        }
    }
}
