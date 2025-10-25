FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BuberDinner.Api/BuberDinner.Api.csproj", "BuberDinner.Api/"]
COPY ["BuberDinner.Contracts/BuberDinner.Contracts.csproj", "BuberDinner.Contracts/"]
COPY ["BuberDinner.Application/BuberDinner.Application.csproj", "BuberDinner.Application/"]
COPY ["BuberDinner.Domain/BuberDinner.Domain.csproj", "BuberDinner.Domain/"]
COPY ["BuberDinner.Infrastructure/BuberDinner.Infrastructure.csproj", "BuberDinner.Infrastructure/"]
RUN dotnet restore "BuberDinner.Api/BuberDinner.Api.csproj"
COPY . .
WORKDIR "/src/BuberDinner.Api"
RUN dotnet build "./BuberDinner.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BuberDinner.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BuberDinner.Api.dll"]
