using System;
using System.Threading.Tasks;
using TimeBooker.Model.Entities;
using TimeBooker.Model.Repositories;

namespace TimeBooker.Model.Services
{
    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _sessionRepository;

        public SessionService(ISessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        public async Task<string> CreateNewSessionAsync(int userId)
        {
            var id = Guid.NewGuid().ToString();
            var expirationDate = DateTime.Now.AddMinutes(10);

            var session = new Session()
            {
                SessionId = id,
                UserId = userId,
                ExpirationDate = expirationDate
            };

            await _sessionRepository.InsertAsync(session);

            return session.SessionId;
        }

        public async Task DeleteSessionAsync(string sessionId)
        {
            await DeleteExpiredSessionsAsync();
            await _sessionRepository.DeleteAsync(sessionId);
        }

        public async Task<bool> ValidateSessionAsync(string sessionId, int userId)
        {
            await DeleteExpiredSessionsAsync();

            var session = await _sessionRepository.GetAsync(sessionId);
            if(session is null) return false;
            if(session.ExpirationDate <  DateTime.Now) return false;
            if(session.UserId != userId) return false;

            return true;
        }

        private async Task DeleteExpiredSessionsAsync() => await _sessionRepository.DeleteExpiredAsync();
    }
}
