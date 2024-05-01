using System.Threading.Tasks;
using TimeBooker.Model.Entities;

namespace TimeBooker.Model.Repositories
{
    public interface ISessionRepository
    {
        Task InsertAsync(Session session);
        Task<Session> GetAsync(string sessionId);
        Task DeleteAsync(string sessionId);
        Task DeleteExpiredAsync();
    }
}
