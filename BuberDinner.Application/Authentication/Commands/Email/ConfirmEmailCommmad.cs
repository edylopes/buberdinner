using BuberDinner.Application.Authentication.Common.Dto;
using BuberDinner.Application.Common.Dto;
using BuberDinner.Domain.Common.Errors;
using MediatR;
using OneOf;

namespace BuberDinner.Application.Authentication.Commands.Email;

public record ConfirmEmailCommand(Guid UserId) : IRequest<OneOf<EmailConfirmed, AppError>>;
