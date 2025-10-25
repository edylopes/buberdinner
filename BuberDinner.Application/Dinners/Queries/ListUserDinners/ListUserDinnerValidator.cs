

namespace BuberDinner.Application.Dinners.Queries.ListUserDinners;

public class ListUserDinnerValidator : AbstractValidator<ListUserDinnersQuery>
{

    public ListUserDinnerValidator()
    {
            RuleFor(d => d.UserId)
            .NotEmpty()
            .WithMessage("UserId is required");
    }
}