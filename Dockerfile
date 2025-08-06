FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

COPY ["IlnarApp.Domain/IlnarApp.Domain.csproj", "IlnarApp.Domain/"]
COPY ["IlnarApp.Infrastructure/IlnarApp.Infrastructure.csproj", "IlnarApp.Infrastructure/"]
COPY ["IlnarApp.Application/IlnarApp.Application.csproj", "IlnarApp.Application/"]
COPY ["IlnarApp.Api/IlnarApp.Api.csproj", "IlnarApp.Api/"]

RUN dotnet restore "IlnarApp.Api/IlnarApp.Api.csproj"

COPY . .

WORKDIR "/IlnarApp.Api"

RUN dotnet build "IlnarApp.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "IlnarApp.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IlnarApp.Api.dll"]