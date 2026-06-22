FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Directory.Build.props global.json ./
COPY BalkanAir.sln ./
COPY src/BalkanAir.Common/BalkanAir.Common.csproj src/BalkanAir.Common/
COPY src/BalkanAir.Domain/BalkanAir.Domain.csproj src/BalkanAir.Domain/
COPY src/BalkanAir.Data/BalkanAir.Data.csproj src/BalkanAir.Data/
COPY src/BalkanAir.Services/BalkanAir.Services.csproj src/BalkanAir.Services/
COPY src/BalkanAir.Api/BalkanAir.Api.csproj src/BalkanAir.Api/
COPY src/BalkanAir.Web/BalkanAir.Web.csproj src/BalkanAir.Web/
COPY tests/BalkanAir.SmokeTests/BalkanAir.SmokeTests.csproj tests/BalkanAir.SmokeTests/
COPY tests/BalkanAir.Services.Tests/BalkanAir.Services.Tests.csproj tests/BalkanAir.Services.Tests/
COPY tests/BalkanAir.Api.IntegrationTests/BalkanAir.Api.IntegrationTests.csproj tests/BalkanAir.Api.IntegrationTests/
RUN dotnet restore BalkanAir.sln

COPY . .

FROM build AS publish-api
RUN dotnet publish src/BalkanAir.Api/BalkanAir.Api.csproj -c Release -o /app/api --no-restore

FROM build AS publish-web
RUN dotnet publish src/BalkanAir.Web/BalkanAir.Web.csproj -c Release -o /app/web --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS api
WORKDIR /app
COPY --from=publish-api /app/api .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
HEALTHCHECK --interval=30s --timeout=5s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1
ENTRYPOINT ["dotnet", "BalkanAir.Api.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS web
WORKDIR /app
COPY --from=publish-web /app/web .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "BalkanAir.Web.dll"]
