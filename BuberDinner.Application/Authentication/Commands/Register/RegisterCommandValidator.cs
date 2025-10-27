
using FluentValidation;

namespace BuberDinner.Application.Authentication.Commands.Register
{
  public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
  {
    public RegisterCommandValidator()
    {
      RuleFor(c => c.Email)
     .NotEmpty()
     .WithMessage("Email is required");


      RuleFor(c => c.Email)
        .EmailAddress()
        .WithMessage("Given email is not valid");


      RuleFor(c => c.Password)
        .NotEmpty()
        .WithMessage("Password is required");

      RuleFor(c => c.Password)
        .MinimumLength(6)
        .WithMessage("Passoword must pelo at least 6 characteres");

      RuleFor(c => c.FirstName)
        .NotEmpty()
        .WithMessage("Name is required");

      RuleFor(c => c.FirstName)
        .MinimumLength(3)
        .WithMessage("First name must be at least 3 characters");


      RuleFor(c => c.LastName)
     .NotEmpty()
     .WithMessage("Name is required");

      RuleFor(c => c.LastName)
     .MinimumLength(3)
     .WithMessage("First name must be at least 3 characters");

    }
  }
}