using BuberDinner.Application.Authentication.Common;
using BuberDinner.Domain.Common.Errors;
using MediatR;
using OneOf;

namespace BuberDinner.Application.Authentication.Commands.Register;

public record RegisterCommand(string firstName, string lastName, string email, string password)
    : IRequest<OneOf<AuthenticationResult, AppError>>;