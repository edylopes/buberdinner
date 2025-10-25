using BuberDinner.Application.Common.Dto.Dinners;
using BuberDinner.Application.Common.Interfaces;

namespace BuberDinner.Application.Dinners.Queries.ListUserDinners;

public record ListUserDinnersQuery(Guid UserId, bool Active ) : IQuery<List<DinnerDto>>;