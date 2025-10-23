
using FluentValidation;

namespace BuberDinner.Application.Authentication.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {

        RuleFor(c => c.Email)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Given email is not valid");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required");
    }

}
