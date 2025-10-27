using BuberDinner.Application.Common.Dto.Dinners;
using BuberDinner.Domain.Entities.Dinners;
using Mapster;

namespace BuberDinner.Application.Common.Mapping;

public class DinnerConfig : IRegister
{

    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Dinner, DinnerDto>()
            .Map(dest => dest.HostName, src => src.Host.Name)
            .Map(dest => dest.Guests,
                src => src.Guests.Select(g => g.Name).ToList())
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.CurrentGuests, src => src.Guests.Count)
            .TwoWays();
    }
}