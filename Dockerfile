FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Directory.Build.props global.json ./
COPY BalkanAir.sln ./
COPY src/BalkanAir.Common/BalkanAir.Common.csproj src/BalkanAir.Common/
COPY src/BalkanAir.Domain/BalkanAir.Domain.csproj src/BalkanAir.Domain/
COPY src/BalkanAir.Data/BalkanAir.Data.csproj src/BalkanAir.Data/
COPY src/BalkanAir.Services/BalkanAir.Services.csproj src/BalkanAir.Services/
COPY src/BalkanAir.Api/BalkanAir.Api.csproj src/BalkanAir.Api/
COPY tests/BalkanAir.SmokeTests/BalkanAir.SmokeTests.csproj tests/BalkanAir.SmokeTests/
COPY tests/BalkanAir.Services.Tests/BalkanAir.Services.Tests.csproj tests/BalkanAir.Services.Tests/
RUN dotnet restore BalkanAir.sln

COPY . .
RUN dotnet publish src/BalkanAir.Api/BalkanAir.Api.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
HEALTHCHECK --interval=30s --timeout=5s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1
ENTRYPOINT ["dotnet", "BalkanAir.Api.dll"]
