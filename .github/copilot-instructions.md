# Copilot Instructions for BuberDinner API

## Project Overview

-   **BuberDinner** is a modular .NET 6 API for dinner event management, with DDD-inspired structure: `Domain`, `Application`, `Infrastructure`, `Contracts`, and `Api` layers.
-   **Authentication** is a primary feature, using JWT and refresh tokens, with endpoints for register/login under `api/v1/auth`.
-   **CQRS** and **MediatR** are used for command/query separation and request handling.
-   **Mapster** is used for object mapping between layers and DTOs.

## Key Architectural Patterns

-   **Domain Layer**: Core business logic, entities (e.g., `User`, `Dinner`), value objects, and domain errors.
-   **Application Layer**: Service interfaces, MediatR commands/queries, and authentication logic. Uses `OneOf` for result/error handling.
-   **Infrastructure Layer**: Persistence (in-memory and EF Core), authentication (JWT), and service implementations. See `DependencyInjection.cs` for service registration.
-   **Api Layer**: Controllers, filters, error mapping, and result wrappers. Custom error handling via `ErrorHandlingFilterAttribute` and `ErrorMapper`.

## Developer Workflows

-   **Build**: `dotnet build BuberDinner.Api/BuberDinner.Api.csproj` or use VS Code task `build`.
-   **Run/Debug**: Use `dotnet watch run --project BuberDinner.Api/BuberDinner.Api.csproj` or VS Code task `watch`. Debug config in `.vscode/launch.json`.
-   **Test Auth Endpoints**: Use `.http` files in `Requests/` or `BuberDinner.Api/Requests/` for manual API testing.
-   **Configuration**: App settings in `BuberDinner.Api/appsettings.json` and `appsettings.Development.json`.

## Project-Specific Conventions

-   **Error Handling**: All errors are mapped to HTTP responses using custom error types in `Domain/Common/Errors` and mapped in `Api/Common/Errors/ErrorMapper.cs`.
-   **Result Wrapping**: API responses use `ResponseResult<T>` and `ResultOkay` for consistent output and header/cookie management.
-   **Refresh Tokens**: Users can have up to 5 active refresh tokens; oldest is removed on overflow (`User.AddRefreshToken`).
-   **Mapping**: All DTO/entity mapping is via Mapster, with config in `Api/Common/Mapping` and `Application/Common/Mapping`.
-   **Rate Limiting**: Configured via `AspNetCoreRateLimit` in `appsettings.json` and DI setup.

## Integration Points

-   **External Packages**: MediatR, Mapster, OneOf, BCrypt.Net, AspNetCoreRateLimit, EntityFrameworkCore.
-   **EF Core**: See `Infrastructure/Persistence/AppDbContext.cs` and related configuration for DB context and entity setup.
-   **JWT**: Configured in `Infrastructure/Authentication/JwtTokenGenerator.cs` and DI.

## Examples

-   **Register a user**: `POST /api/v1/auth/register` with JSON body (see `Requests/Register.http`).
-   **Login**: `POST /api/v1/auth/login` (see `Requests/Login.http`).
-   **Error mapping**: See `Api/Common/Errors/ErrorMapper.cs` for how domain errors are translated to HTTP responses.

## Where to Look

-   **Controllers**: `BuberDinner.Api/Controllers/`
-   **Domain Models**: `BuberDinner.Domain/Entities/`
-   **Application Services**: `BuberDinner.Application/Services/`
-   **Mapping Config**: `BuberDinner.Api/Common/Mapping/`, `BuberDinner.Application/Common/Mapping/`
-   **Error Handling**: `BuberDinner.Api/Filters/`, `BuberDinner.Api/Common/Errors/`
-   **Persistence**: `BuberDinner.Infrastructure/Persistence/`

For new features, follow the CQRS pattern, add new commands/queries in `Application`, map results in `Api`, and update DI as needed. Use provided `.http` files for endpoint testing.
