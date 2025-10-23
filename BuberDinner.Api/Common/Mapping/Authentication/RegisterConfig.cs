using BuberDinner.Application.Authentication.Common;
using BuberDinner.Contracts.Authentication;
using Mapster;

namespace BuberDinner.Api.Common.Mapping.Authentication;

public class AuthResponseMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {

        config.NewConfig<AuthenticationResult, AuthResponse>()
            .Map(dest => dest.Id, src => src.User.Id)
            .Map(dest => dest.FirstName, src => src.User.FirstName)
            .Map(dest => dest.LastName, src => src.User.LastName)
            .Map(dest => dest.Email, src => src.User.Email)
            .Map(dest => dest.Roles, src => src.User.Roles.Select(r => r.ToString()).ToArray());

    }
}
