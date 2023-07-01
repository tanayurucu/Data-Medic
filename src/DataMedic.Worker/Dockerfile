FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base

WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/DataMedic.Worker/DataMedic.Worker.csproj", "src/DataMedic.Worker/"]
COPY ["src/DataMedic.Application/DataMedic.Application.csproj", "src/DataMedic.Application/"]
COPY ["src/DataMedic.Domain/DataMedic.Domain.csproj", "src/DataMedic.Domain/"]
COPY ["src/DataMedic.Infrastructure/DataMedic.Infrastructure.csproj", "src/DataMedic.Infrastructure/"]
COPY ["src/DataMedic.Persistence/DataMedic.Persistence.csproj", "src/DataMedic.Persistence/"]
RUN dotnet restore "src/DataMedic.Worker/DataMedic.Worker.csproj"
COPY . .
WORKDIR "/src/src/DataMedic.Worker"
RUN dotnet build "DataMedic.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DataMedic.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=publish /src/src/DataMedic.Infrastructure/Email/Templates ../DataMedic.Infrastructure/Email/Templates
ENTRYPOINT ["dotnet", "DataMedic.Worker.dll"]