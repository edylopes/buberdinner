using BuberDinner.Application.Common.Dto.Dinners;
using BuberDinner.Application.Common.Interfaces;

namespace BuberDinner.Application.Dinners.Queries;

public record ListUserDinnersQuery(Guid UserId ) : IQuery<List<DinnerDto>>;