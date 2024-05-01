using System.Threading.Tasks;

namespace TimeBooker.Model.Services
{
    public interface ISessionService
    {
        Task<string> CreateNewSessionAsync(int userId);
        Task<bool> ValidateSessionAsync(string sessionId, int userId);
        Task DeleteSessionAsync(string sessionId);
    }
}
