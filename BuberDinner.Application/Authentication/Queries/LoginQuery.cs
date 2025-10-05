using BuberDinner.Application.Authentication.Common;
using BuberDinner.Domain.Common.Errors;
using MediatR;
using OneOf;

namespace BuberDinner.Application.Authentication.Queries;

public record LoginQuery(string email, string password)
    : IRequest<OneOf<AuthenticationResult, AppError>>;
