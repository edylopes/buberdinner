
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace BuberDinner.Infrastructure.Authentication
{
    public static class JwtDepencyInjection
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.JWT));
            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
            services.ToAuth(configuration);

            return services;
        }

        public static IServiceCollection ToAuth(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var section = configuration.GetSection(JwtSettings.JWT);

            var key = section["SecretKey"]!;
            var aud = section["Audience"]!;
            var issuer = section["Issuer"]!;

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(5),
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidAudience = aud,
                    ValidIssuer = issuer,

                };

            });

            return services;
        }
    }
}
