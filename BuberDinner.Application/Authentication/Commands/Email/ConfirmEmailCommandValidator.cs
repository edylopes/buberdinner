using System.IdentityModel.Tokens.Jwt;

namespace BuberDinner.Application.Authentication.Commands.Email;

public class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
{
    public ConfirmEmailCommandValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty()
            .WithMessage("Token is required")
            .MustBeAValidJwt();
    }
}

public static class CustomValidators
{
    public static IRuleBuilderOptions<T, string> MustBeAValidJwt<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(token =>
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;

            var parts = token.Split('.');
            if (parts.Length != 3 && parts.Length != 5)
                return false;

            var handler = new JwtSecurityTokenHandler();
            return handler.CanReadToken(token);
        })
        .WithMessage("Invalid or malformed JWT token.");
    }
}

