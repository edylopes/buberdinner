using BuberDinner.Application.Authentication.Common;
using BuberDinner.Contracts.Authentication;
using Mapster;

namespace BuberDinner.Api.Common.Mapping.Authentication;

public class AuthResponseMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        
        config.NewConfig<AuthenticationResult, AuthResponse>()
            .Map(dest => dest.id, src => src.User.Id)
            .Map(dest => dest.firstName, src => src.User.FirstName)
            .Map(dest => dest.lastName, src => src.User.LastName)
            .Map(dest => dest.email, src => src.User.Email)
            .Map(dest => dest.role, src => src.User.Role);
            
    }
}