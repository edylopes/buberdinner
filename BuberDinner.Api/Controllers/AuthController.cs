using BuberDinner.Api.Common.Errors;
using BuberDinner.Api.Extensions;
using BuberDinner.Api.Extensions.Auth;
using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Queries;
using BuberDinner.Contracts.Authentication;
using MapsterMapper;
using MediatR;


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
        return result.ToAuthResponse(_mapper, HttpContext);
    }
    [HttpPost(template: "login")]
    public async Task<IActionResult> Login(LoginRequest req)
    {
        var result = await _mediator.Send(_mapper.Map<LoginQuery>(req));

        return result.ToResponseResult(
           user => Ok(_mapper.Map<AuthResponse>(user)),
           HttpContext);
    }
}
