using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Application.Authentication.Commands.Login;
using BuberDinner.Contracts.Authentication;
using BuberDinner.Domain.Entities;
using Mapster;

namespace BuberDinner.Application.Common.Mapping;

public class AuthRegisterMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, AuthenticationResult>()
            .ConstructUsing(src => new AuthenticationResult(
                src,
                string.Empty,
                src.RefreshTokens!.Last().Token)
            );

        config.NewConfig<LoginRequest, LoginCommand>()
                .Map(dest => dest.email, src => src.email)
                .Map(dest => dest.password, src => src.password)
                .TwoWays();

        config.NewConfig<RegisterRequest, RegisterCommand>()
               .ConstructUsing(src =>
               new RegisterCommand(src.firstName, src.lastName, src.email, src.password));

    }
}