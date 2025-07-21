using BurberDinner.Api.Filters;
using BurberDinner.Api.Middleware;
using BurberDinner.Api.Utils;
using BurberDinner.Application;
using BurberDinner.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

{
    builder.Services
        .AddApplication()
        .AddInfrastructure(builder.Configuration);

    // Register the error handling filter globally
    // ExceptionMappingRegistry.RegisterDefaults();

    builder.Services.AddControllers(options =>
    {
        options.Filters.Add<ErrorHandlingFilterAttribute>();
    });

}

var app = builder.Build();
{
    // app.UseMiddleware<ErrorHandlingMiddleware>();

    app.UseHttpsRedirection();
    app.MapControllers();
    app.Run();
}