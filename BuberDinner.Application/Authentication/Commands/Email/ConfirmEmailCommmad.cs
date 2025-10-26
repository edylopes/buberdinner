using BuberDinner.Application.Common.Dto.Email.Enums;

namespace BuberDinner.Application.Authentication.Commands.Email;

public record ConfirmEmailCommand(string Token) : IRequest<OneOf<string, EmailConfirmationError>>;
