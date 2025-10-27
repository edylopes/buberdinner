using BuberDinner.Application.Authentication.Common;
using BuberDinner.Application.Common.Interfaces;
using BuberDinner.Domain.Common.Errors;

namespace BuberDinner.Application.Authentication.Commands.Login;

public record LoginCommand(string Email, string Password)
    : ICommand<OneOf<AuthenticationResult, AppError>>;
