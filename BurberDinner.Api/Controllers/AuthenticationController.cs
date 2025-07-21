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
            var authResult = await _authenticationService.Register(
                req.FirstName,
                req.LastName,
                req.Email,
                req.Password
                , "User" // Assuming a default role, you can modify this as needed
            );

            _logger.LogInformation("User registered: {Email} {date: MMM dd, yyyy}", req.Email, DateTime.UtcNow);
            return Ok(
                new AuthenticationResponse(authResult.Id, authResult.FirstName, authResult.LastName,
                    authResult.Email, authResult.Role)
            );
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest req)
        {
            var authResult = await _authenticationService.Login(req.Email, req.Password);
            if (authResult == null)
            {
                return Unauthorized(new { Message = "Invalid email or password." });
            }

            Response.Cookies.Append("refreshToken", authResult.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(7) // Set the expiration as needed
            });

            Response.Headers["Authorization"] = $"Bearer {authResult.Token}";

            return Ok(new AuthenticationResponse(authResult.Id, authResult.FirstName,
                authResult.LastName, authResult.Email, authResult.Role));

        }
    }
}