using BuberDinner.Application.Authentication.Common;
using BuberDinner.Domain.Entities;
using Mapster;

namespace BuberDinner.Application.Common.Mapping;

public class AuthRegisterMapping : IRegister    
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, AuthenticationResult>()
            .Map(dest => dest.AccessToken, src => string.Empty)
            .Map(dest => dest.RefreshToken, src => src.RefreshTokens!.Last().Token)
            .ConstructUsing(src => new AuthenticationResult(
                src,
                string.Empty,
                src.RefreshTokens!.Last().Token)
            );
    }
}