FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["backend/ContractFlow.Api/ContractFlow.Api.csproj", "backend/ContractFlow.Api/"]
COPY ["backend/ContractFlow.Application/ContractFlow.Application.csproj", "backend/ContractFlow.Application/"]
COPY ["backend/ContractFlow.Domain/ContractFlow.Domain.csproj", "backend/ContractFlow.Domain/"]
COPY ["backend/ContractFlow.Infrastructure/ContractFlow.Infrastructure.csproj", "backend/ContractFlow.Infrastructure/"]

RUN dotnet restore "backend/ContractFlow.Api/ContractFlow.Api.csproj"

COPY . .

WORKDIR "/src/backend/ContractFlow.Api"

RUN dotnet publish "ContractFlow.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "ContractFlow.Api.dll"]