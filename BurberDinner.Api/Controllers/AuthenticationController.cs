using BurberDinner.Api.Extensions;
using BurberDinner.Api.Filters;
using BurberDinner.Application.Services.Authentication;
using BurberDinner.Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace BurberDinner.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    //  [ErrorHandlingFilterAttribute] Apply the error handling filter globally for this controller
    public class AuthenticationController : Controller
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService, ILogger<AuthenticationController> logger)

        {
            _authenticationService = authenticationService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest req)
        {
            var authResult = await _authenticationService.Register(req.FirstName, req.LastName, req.Email, req.Password);
            return authResult.ToActionResult(this);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest req)
        {

            var authResult = await _authenticationService.Login(req.Email, req.Password);
            return authResult.ToActionResult(this);

        }
    }
}