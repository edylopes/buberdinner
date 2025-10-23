using BuberDinner.Application.Authentication.Common;
using BuberDinner.Domain.Common.Errors;
using MediatR;
using OneOf;

namespace BuberDinner.Application.Authentication.Commands.Login;

public record LoginCommand(string Email, string Password)
    : IRequest<OneOf<AuthenticationResult, AppError>>;
