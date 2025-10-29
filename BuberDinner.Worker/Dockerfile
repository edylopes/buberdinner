# Etapa base (runtime)
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copia os projetos e restaura dependências
COPY ["BuberDinner.Worker/BuberDinner.Worker.csproj", "BuberDinner.Worker/"]
COPY ["BuberDinner.Infrastructure/BuberDinner.Infrastructure.csproj", "BuberDinner.Infrastructure/"]
COPY ["BuberDinner.Application/BuberDinner.Application.csproj", "BuberDinner.Application/"]
COPY ["BuberDinner.Domain/BuberDinner.Domain.csproj", "BuberDinner.Domain/"]
COPY ["BuberDinner.Contracts/BuberDinner.Contracts.csproj", "BuberDinner.Contracts/"]

RUN dotnet restore "BuberDinner.Worker/BuberDinner.Worker.csproj"

# Copia o código e compila
COPY . .
WORKDIR "/src/BuberDinner.Worker"
RUN dotnet publish "BuberDinner.Worker.csproj" -c Release -o /app/publish

# Etapa final (imagem enxuta)
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BuberDinner.Worker.dll"]
