using BuberDinner.Application.Common.Dto.Dinners;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Application.Common.Interfaces.Persistence.Dinners;
using MapsterMapper;
using MediatR;

namespace BuberDinner.Application.Dinners.Queries;

public class ListUserDinnersQueryHandler : IRequestHandler<ListUserDinnersQuery, List<DinnerDto>>
{
    private readonly IMapper _mapper;
    private readonly IDinnerRepository  _dinnerRepository;

    public ListUserDinnersQueryHandler(IUnitOfWork uow, IMapper mapper, IDinnerRepository dinnerRepository)
    {
        _mapper = mapper;
        _dinnerRepository = dinnerRepository;
    }
    public Task<List<DinnerDto>> Handle(ListUserDinnersQuery request, CancellationToken cancellationToken)
    {
        var dinners = _dinnerRepository.ListUserDinnersAsync(request.UserId, active: true);
         var result =  _mapper.Map<List<DinnerDto>>(dinners);
         return  Task.FromResult(result);
    }
}  