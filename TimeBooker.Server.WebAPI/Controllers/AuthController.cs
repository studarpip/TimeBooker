using Microsoft.AspNetCore.Mvc;
using TimeBooker.Model.Entities;
using TimeBooker.Model.Entities.Requests;
using TimeBooker.Model.Entities.Responses;
using TimeBooker.Model.Services;

namespace TimeBooker.Server.WebAPI.Controllers
{
    [Controller, Route("/auth")]
    public class AuthController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ServerResult<int>> RegisterAsync([FromBody] RegistrationRequest request) => await _authService.RegisterAsync(request);

        [HttpPost("login")]
        public async Task<ServerResult<LoginResponse>> LoginAsync([FromBody] LoginRequest request) => await _authService.LoginAsync(request);

        [HttpPost("logout")]
        public async Task<ServerResult> LogoutAsync([FromBody] LogoutRequest request) => await _authService.LogoutAsync(request);
    }
}
