
using BuberDinner.Application.Authentication.Dto;
using BuberDinner.Domain.Common.Errors;
using MediatR;
using OneOf;

namespace BuberDinner.Application.Authentication.Commands.Email;

public record ConfirmEmailCommand(Guid UserId) : IRequest<OneOf<EmailConfirmed, AppError>>;
