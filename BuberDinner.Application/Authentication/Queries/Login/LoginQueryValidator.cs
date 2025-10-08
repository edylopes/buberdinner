
using FluentValidation;

namespace BuberDinner.Application.Authentication.Queries.Login;

public class LoginQueryValidator : AbstractValidator<LoginQuery>
{
    public LoginQueryValidator()
    {

        RuleFor(c => c.email)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Given email is not valid");

        RuleFor(x => x.password)
            .NotEmpty().WithMessage("Password is required");
    }

}
