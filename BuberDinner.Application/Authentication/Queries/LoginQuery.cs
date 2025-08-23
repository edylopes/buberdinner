using BuberDinner.Application.Services.Authentication.Common;
using BuberDinner.Domain.Common.Errors;
using MediatR;
using OneOf;

namespace BuberDinner.Application.Authentication.Queries;

public record LoginQuery(string Email, string Password)
    : IRequest<OneOf<AuthenticationResult, AppError>>;
