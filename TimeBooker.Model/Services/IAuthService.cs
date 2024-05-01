using System.Threading.Tasks;
using TimeBooker.Model.Entities;
using TimeBooker.Model.Entities.Requests;
using TimeBooker.Model.Entities.Responses;

namespace TimeBooker.Model.Services
{
    public interface IAuthService
    {
        Task<ServerResult<LoginResponse>> LoginAsync(LoginRequest request);
        Task<ServerResult<int>> RegisterAsync(RegistrationRequest request);
        Task<ServerResult> LogoutAsync(LogoutRequest request);
    }
}
