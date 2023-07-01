FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base

WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/DataMedic.Presentation/DataMedic.Presentation.csproj", "src/DataMedic.Presentation/"]
COPY ["src/DataMedic.Application/DataMedic.Application.csproj", "src/DataMedic.Application/"]
COPY ["src/DataMedic.Domain/DataMedic.Domain.csproj", "src/DataMedic.Domain/"]
COPY ["src/DataMedic.Contracts/DataMedic.Contracts.csproj", "src/DataMedic.Contracts/"]
COPY ["src/DataMedic.Persistence/DataMedic.Persistence.csproj", "src/DataMedic.Persistence/"]
COPY ["src/DataMedic.Infrastructure/DataMedic.Infrastructure.csproj", "src/DataMedic.Infrastructure/"]
COPY ["src/DataMedic.Web/DataMedic.Web.csproj", "src/DataMedic.Web/"]
RUN dotnet restore "src/DataMedic.Web/DataMedic.Web.csproj"
COPY . .
WORKDIR "/src/src/DataMedic.Web"
RUN dotnet build "DataMedic.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DataMedic.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DataMedic.Web.dll"]
