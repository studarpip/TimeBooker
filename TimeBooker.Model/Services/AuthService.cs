using System;
using System.Threading.Tasks;
using TimeBooker.Model.Entities;
using TimeBooker.Model.Entities.Requests;
using TimeBooker.Model.Entities.Responses;
using TimeBooker.Model.Helpers;

namespace TimeBooker.Model.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly ISessionService _sessionService;

        public AuthService(IUserService userService, ISessionService sessionService)
        {
            _userService = userService;
            _sessionService = sessionService;
        }

        public async Task<ServerResult<LoginResponse>> LoginAsync(LoginRequest request)
        {
            try
            {
                request.Password = request.Password.Hash();
                var user = await _userService.LoginAsync(request);
                if (user is null)
                    return new() { Success = false, Message = "No user with these credentials!" };

                var sessionId = await _sessionService.CreateNewSessionAsync(user.Id);

                return new() { Success = true, Data = new() { UserId = user.Id, SessionId = sessionId } };
            }
            catch (Exception ex)
            {
                return new() { Success = false, Message = ex.Message };
            }
        }

        public async Task<ServerResult> LogoutAsync(LogoutRequest request)
        {
            try
            {
                await _sessionService.DeleteSessionAsync(request.SessionId);
                return new() { Success = true };
            }
            catch (Exception ex)
            {
                return new() { Success = false, Message = ex.Message };
            }
        }

        public async Task<ServerResult<int>> RegisterAsync(RegistrationRequest request)
        {
            try
            {
                var emailExists = await _userService.EmailExistsAsync(request.Email);
                if (emailExists)
                    return new() { Success = false, Message = "Email is taken!" };

                var newUser = new User
                {
                    Name = request.Name,
                    Email = request.Email,
                    Password = request.Password.Hash(),
                };

                var id = await _userService.CreateUserAsync(newUser);

                return new() { Success = true, Data = id };
            }
            catch (Exception ex)
            {
                return new() { Success = false, Message = ex.Message };
            }
        }
    }
}
