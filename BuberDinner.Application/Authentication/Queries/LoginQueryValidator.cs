
using FluentValidation;

namespace BuberDinner.Application.Authentication.Queries;

public class LoginQueryValidator : AbstractValidator<LoginQuery>
{
    public LoginQueryValidator()
    {
        RuleFor(x => x.email)
         .NotEmpty()
         .EmailAddress()
         .WithMessage("Given email is not valid");


        RuleFor(x => x.password).NotEmpty();
    }

}
