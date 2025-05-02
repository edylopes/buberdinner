using BurberDinner.Application.Services.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

{
    builder.Services.AddControllers();
    builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
}


var app = builder.Build();
{
    app.UseHttpsRedirection();
    app.MapControllers();
    app.Run();
}