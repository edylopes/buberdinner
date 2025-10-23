using BuberDinner.Application.Common.Dto.Dinners;
using BuberDinner.Domain.Entities;

namespace BuberDinner.Application.Authentication.Queries.Dinners;

public record ListUserDinnersQuery(Guid UserId ) : IQuery<List<DinnerDto>>;