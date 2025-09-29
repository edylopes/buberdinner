using BuberDinner.Application.Authentication.Common;
using BuberDinner.Contracts.Authentication;
using Mapster;

namespace BuberDinner.Api.Common.Mapping.Authentication;

public class AuthResponseMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {

        config.NewConfig<AuthenticationResult, AuthResponse>()
            .Map(dest => dest.id, src => src.user.Id)
            .Map(dest => dest.firstName, src => src.user.FirstName)
            .Map(dest => dest.lastName, src => src.user.LastName)
            .Map(dest => dest.email, src => src.user.Email)
            .Map(dest => dest.roles, src => src.user.Roles.Select(r => r.ToString()).ToArray());

    }
}
