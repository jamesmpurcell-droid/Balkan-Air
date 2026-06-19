# Balkan Air — .NET 8 Modernisation

> Fully modernised from .NET Framework 4.5.2 / ASP.NET Web Forms to **.NET 8 / ASP.NET Core**.
> Migration complete — legacy code has been removed.

## Architecture

```
src/
  BalkanAir.Common      → Shared constants, error messages, user roles
  BalkanAir.Domain      → 20 entities + 6 enums (clean POCOs, no EF dependencies)
  BalkanAir.Data        → EF Core 8 DbContext + IdentityDbContext, 20 fluent entity configurations
  BalkanAir.Services    → 21 async service implementations (generic CRUD + specialised)
  BalkanAir.Api         → ASP.NET Core Web API (12 controllers, Swagger/OpenAPI, Serilog, OpenTelemetry)
  BalkanAir.Web         → ASP.NET Core MVC (Bootstrap 5, Razor views, Identity auth)

tests/
  BalkanAir.SmokeTests               → Toolchain smoke test
  BalkanAir.Services.Tests           → 13 xUnit tests (EF Core InMemory)
  BalkanAir.Api.IntegrationTests     → 11 WebApplicationFactory integration tests
```

## Build & Test

```bash
# Requires .NET 8 SDK (pinned in global.json)
dotnet build BalkanAir.sln
dotnet test  BalkanAir.sln          # 25 tests total
```

## Run with Docker

```bash
docker compose up -d                 # SQL Server 2022 + API on port 8080
open http://localhost:8080/swagger    # Swagger UI
```

## Features

- **Flight booking flow** — search, select flight, confirm, view bookings
- **User authentication** — ASP.NET Core Identity (register, login, logout)
- **User profiles** — edit name, phone, DOB, nationality, address
- **Administration dashboard** — role-based access, 15 entity management views, soft-delete
- **REST API** — 12 controllers with full CRUD, Swagger/OpenAPI documentation
- **Structured logging** — Serilog with console sink, request logging
- **Distributed tracing** — OpenTelemetry with ASP.NET Core + HttpClient instrumentation
- **Health checks** — `/health` endpoint for container orchestration

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
| OWIN + ASP.NET Identity 2 | ASP.NET Core Identity |
| No structured logging | Serilog + OpenTelemetry |
| No integration tests | WebApplicationFactory + InMemory DB |

## CI

GitHub Actions: build + test on every push/PR (`.github/workflows/ci.yml`).

## Original Project

This application simulates an online reservation system that allows users to search for flights, book seats in different travel classes, receive notifications for new flights and news, manage profiles, and access data via the REST API.
