using BuberDinner.Application.Common.Dto.Dinners;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Application.Common.Interfaces.Persistence.Dinners;
using MapsterMapper;

namespace BuberDinner.Application.Dinners.Queries.ListUserDinners;

public class ListUserDinnersQueryHandler : IRequestHandler<ListUserDinnersQuery, List<DinnerDto>>
{
    private readonly IMapper _mapper;
    private readonly IDinnerRepository  _dinnerRepository;

    public ListUserDinnersQueryHandler(IUnitOfWork uow, IMapper mapper, IDinnerRepository dinnerRepository)
    {
        _mapper = mapper;
        _dinnerRepository = dinnerRepository;
    }
    public async Task<List<DinnerDto>> Handle(ListUserDinnersQuery request, CancellationToken cancellationToken)
    {
        var dinners =  await _dinnerRepository.ListUserDinnersAsync(request.UserId, active: true);
        return dinners;
    }
}  