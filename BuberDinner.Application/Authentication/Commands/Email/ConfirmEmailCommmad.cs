using BuberDinner.Application.Common.Dto;
using BuberDinner.Application.Common.Interfaces;
using BuberDinner.Domain.Common.Errors;

namespace BuberDinner.Application.Authentication.Commands.Email;

public record ConfirmEmailCommand(string Token, string? email = null) : ICommand<OneOf<EmailConfirmed, AppError>>;
