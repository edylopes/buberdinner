using BurberDinner.Application.Services.Authentication;
using BurberDinner.Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace BurberDinner.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterRequest req)
        {
            var authResult = _authenticationService.Register(
                req.FirstName,
                req.LastName,
                req.Email,
                req.Password
            );

            return Ok(
                new AuthenticationResponse(authResult.Id, authResult.FirstName!, authResult.LastName!,
                    authResult.Email, authResult.Token!)
            );
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest req)
        {
            var authResult = _authenticationService.Login(req.Email, req.Password);
            return Ok(new AuthenticationResponse(authResult.Id, authResult.FirstName!, authResult.LastName!,
                authResult.Email, authResult.Token!));
        }
    }
}