
using FluentValidation;

namespace BuberDinner.Application.Authentication.Commands.Register
{
  public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
  {
    public RegisterCommandValidator()
    {
      RuleFor(c => c.email)
      .NotEmpty()
      .WithMessage("Email is required");


      RuleFor(c => c.email)
        .EmailAddress()
        .WithMessage("Given email is not valid")
        .When(c => !string.IsNullOrWhiteSpace(c.email)); ;


      RuleFor(c => c.password)
        .NotEmpty()
        .WithMessage("Password is required");

      RuleFor(c => c.password)
        .MinimumLength(6)
        .WithMessage("Passoword must pelo at least 6 characteres")
        .When(c => !string.IsNullOrWhiteSpace(c.password));

      RuleFor(c => c.firstName)
        .NotEmpty()
        .WithMessage("Name is required");

      RuleFor(c => c.firstName)
        .MinimumLength(3)
        .WithMessage("First name must be at least 3 characters");


      RuleFor(c => c.lastName)
     .NotEmpty()
     .WithMessage("Name is required");

      RuleFor(c => c.lastName)
     .MinimumLength(3)
     .WithMessage("First name must be at least 3 characters");

    }
  }
}