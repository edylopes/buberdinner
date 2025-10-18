
using BuberDinner.Api.Extensions.Auth;
using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Contracts.Authentication;

using Microsoft.AspNetCore.Authorization;
using MapsterMapper;

using BuberDinner.Application.Authentication.Commands.Login;
using BuberDinner.Application.Services.Authentication;
using BuberDinner.Api.Common.Errors;
namespace BuberDinner.Api.Controllers;


[ApiController]
[Route("api/v1/auth")]

public class AuthController : Controller
{
    private readonly ILogger<AuthController> _logger;
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public AuthController(ILogger<AuthController> logger, ISender mediator, IMapper mapper)
    {
        _mapper = mapper;
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost(template: "register")]
    public async Task<IActionResult> Register(RegisterRequest req)
    {
        var result = await _mediator.Send(_mapper.Map<RegisterCommand>(req));
        return result.ToRegister(_mapper);
    }

    [HttpPost(template: "login")]
    public async Task<IActionResult> Login(LoginRequest req)
    {
        var result = await _mediator.Send(_mapper.Map<LoginCommand>(req));
        return result.ToLogin(_mapper);
    }

    [HttpGet("me"), Authorize(Roles = "User")]
    public IActionResult Me()
    {
        var name = User?.Identity?.Name;
        return Ok(new { name });
    }
    [HttpPost("confirm-email")]

    public async Task<IActionResult> ConfirmEmail(
        [FromQuery] string userId,
        [FromServices] IAuthenticationService service,
        CancellationToken ct)
    {
        if (!Guid.TryParse(userId, out var uid))
            return Problem(title: "Invalid userId", statusCode: 400);

        var result = await service.ConfirmEmailAsync(uid, ct);
        return result.Match(
            success => Ok(success),
            error => ErrorResults.FromError(error)
        );
    }
}
