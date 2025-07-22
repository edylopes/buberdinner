using BuberDinner.Api.Extensions;
using BuberDinner.Api.Filters.Results;
using BuberDinner.Application.Services.Authentication;
using BuberDinner.Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : Controller
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(
            IAuthenticationService authenticationService,
            ILogger<AuthenticationController> logger
        )
        {
            _authenticationService = authenticationService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest req)
        {
            var result = await _authenticationService.Register(
                req.FirstName,
                req.LastName,
                req.Email,
                req.Password
            );
            return result.Match(
                success => new AuthResultWithCookies(success),
                error => error.ToProblemDetails()
            );
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest req)
        {
            var authResult = await _authenticationService.Login(req.Email, req.Password);
            return authResult.ToAuthActionResult(this);
        }
    }
}
