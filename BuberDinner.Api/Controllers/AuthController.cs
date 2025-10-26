
using BuberDinner.Api.Extensions.Auth;
using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Commands.Login;
using BuberDinner.Application.Authentication.Commands.Email;
using BuberDinner.Contracts.Authentication;
using BuberDinner.Api.Extensions;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using FluentEmail.Core;
using BuberDinner.Application.Common.Dto;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.Net;

namespace BuberDinner.Api.Controllers;


[ApiController]
[Route("api/v1/auth")]
[AllowAnonymous]
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

    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string token, CancellationToken ct)
    {
        var result = await _mediator.Send(new ConfirmEmailCommand(token), ct);
        return result.Match(
            sucess => Ok(new EmailConfirmed()),
            error => Problem(title: "Validation Error", detail: error.ToString(), statusCode: 400)
        );
    }
}
