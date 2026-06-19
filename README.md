# Balkan Air — .NET 8 Modernisation

> Fully modernised from .NET Framework 4.5.2 / ASP.NET Web Forms to **.NET 8 / ASP.NET Core**.
> The original source is preserved under [`legacy/`](./legacy) (strangler-fig approach).

## Architecture

```
src/
  BalkanAir.Common      → Shared constants, error messages, user roles
  BalkanAir.Domain      → 20 entities + 6 enums (clean POCOs, no EF dependencies)
  BalkanAir.Data        → EF Core 8 DbContext + 20 fluent entity configurations (SQL Server)
  BalkanAir.Services    → 21 async service implementations (generic CRUD + specialised)
  BalkanAir.Api         → ASP.NET Core Web API (12 controllers, Swagger/OpenAPI)
  BalkanAir.Web         → ASP.NET Core MVC (Bootstrap 5, Razor views)

tests/
  BalkanAir.SmokeTests       → Toolchain smoke test
  BalkanAir.Services.Tests   → 13 xUnit tests (EF Core InMemory)
```

## Build & Test

```bash
# Requires .NET 8 SDK (pinned in global.json)
dotnet build BalkanAir.sln
dotnet test  BalkanAir.sln
```

## Run with Docker

```bash
docker compose up -d          # SQL Server 2022 + API on port 8080
open http://localhost:8080/swagger   # Swagger UI
```

## Key Modernisation Changes

| Legacy (.NET Framework 4.5.2) | Modern (.NET 8) |
|-------------------------------|-----------------|
| ASP.NET Web Forms (52 .aspx pages) | ASP.NET Core MVC + Razor |
| ASP.NET Web API 2 | ASP.NET Core Web API |
| Entity Framework 6 | Entity Framework Core 8 |
| Ninject DI | Built-in `Microsoft.Extensions.DependencyInjection` |
| Newtonsoft.Json | System.Text.Json |
| AppVeyor CI | GitHub Actions |
| Windows-only IIS | Cross-platform, containerised (Docker) |
| OWIN + ASP.NET Identity 2 | ASP.NET Core Identity (planned) |

## CI

GitHub Actions: build + test on every push/PR (`.github/workflows/ci.yml`).

## Original Project

This application simulates an online reservation system that allows users to search for flights, book seats in different travel classes, receive notifications for new flights and news, manage profiles, and access data via the REST API.

## Screenshots (Legacy)

![Home page](./legacy/Screenshots/Home/01.%20Home.png)
![Booking page](./legacy/Screenshots/Booking/01.%20Select%20Flight.png)
![API page](./legacy/Screenshots/API/02.%20API%20Overview.png)
