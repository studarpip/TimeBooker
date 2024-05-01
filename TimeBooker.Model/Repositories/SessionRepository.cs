using System;
using System.Threading.Tasks;
using TimeBooker.Model.Entities;

namespace TimeBooker.Model.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly IRepository _repository;

        public SessionRepository(IRepository repository)
        {
            _repository = repository;
        }

        public async Task DeleteAsync(string sessionId)
        {
            var sql = "DELETE FROM sessions WHERE SessionId = @SessionId";
            await _repository.ExecuteAsync(sql, new { SessionId = sessionId });
        }

        public async Task DeleteExpiredAsync()
        {
            var sql = "DELETE FROM sessions WHERE ExpirationDate <= @DateNow";
            await _repository.ExecuteAsync(sql, new { DateNow = DateTime.Now });
        }

        public async Task<Session> GetAsync(string sessionId)
        {
            var sql = @"SELECT * FROM sessions WHERE SessionId = @SessionId";

            return await _repository.QueryAsync<Session, dynamic>(sql, new { SessionId = sessionId });
        }

        public async Task InsertAsync(Session session)
        {
            var sql = @"
            INSERT INTO sessions (`SessionId`, `UserId`, `ExpirationDate`)
            VALUES (@SessionId, @UserId, @ExpirationDate);";

            await _repository.ExecuteAsync(sql, new
            {
                session.SessionId,
                session.UserId,
                session.ExpirationDate
            });
        }
    }
}
