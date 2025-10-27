using BuberDinner.Application.Authentication.Common;
using BuberDinner.Application.Common.Interfaces;
using BuberDinner.Domain.Common.Errors;

namespace BuberDinner.Application.Authentication.Commands.Register;

public record RegisterCommand(string FirstName, string LastName, string Email, string Password)
    : ICommand<OneOf<AuthenticationResult, AppError>>;