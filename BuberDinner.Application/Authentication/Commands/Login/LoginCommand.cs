using BuberDinner.Application.Authentication.Common;
using BuberDinner.Domain.Common.Errors;
using MediatR;
using OneOf;

namespace BuberDinner.Application.Authentication.Commands.Login;

public record LoginCommand(string email, string password)
    : IRequest<OneOf<AuthenticationResult, AppError>>;
