---
name: testing-balkanair-e2e
description: End-to-end browser testing for the Balkan Air .NET 8 web app. Use when verifying auth flows, admin dashboard, booking, profile, and UI after code changes.
---

# Balkan Air E2E Testing

## Prerequisites

- .NET 8 SDK installed
- Docker running (for SQL Server)
- Chrome browser available

## Local Environment Setup

### 1. Start SQL Server

```bash
docker run -d --name balkanair-sql \
  -e ACCEPT_EULA=Y \
  -e MSSQL_SA_PASSWORD=Your_strong_Pass123 \
  -e MSSQL_PID=Developer \
  -p 1433:1433 \
  mcr.microsoft.com/mssql/server:2022-latest
```

Wait ~10s for SQL Server to be ready. Verify with:
```bash
docker exec balkanair-sql /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P 'Your_strong_Pass123' -No -Q "SELECT 1"
```

### 2. Configure the Web App for Local Testing

In `src/BalkanAir.Web/Program.cs`, you may need two temporary changes:

1. **Add `EnsureCreated()` after `builder.Build()`** if EF migrations aren't set up:
   ```csharp
   var app = builder.Build();
   using (var scope = app.Services.CreateScope())
   {
       var db = scope.ServiceProvider.GetRequiredService<BalkanAir.Data.BalkanAirDbContext>();
       db.Database.EnsureCreated();
   }
   ```

2. **Disable HTTPS redirect** if no local cert is configured:
   ```csharp
   // app.UseHttpsRedirection(); // disabled for local testing
   ```

### 3. Start the Web App

Port 5000 might be in use. Use a different port:
```bash
cd src/BalkanAir.Web
dotnet run --urls "http://0.0.0.0:5050"
```

The app should be accessible at `http://localhost:5050`.

### 4. Seed Roles and Test Admin User

Seed the Identity roles:
```bash
docker exec balkanair-sql /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P 'Your_strong_Pass123' -No -d BalkanAir -I \
  -Q "INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp) VALUES (NEWID(), 'User', 'USER', NEWID()), (NEWID(), 'Administrator', 'ADMINISTRATOR', NEWID());"
```

**Important:** Use `-I` flag with sqlcmd to enable `QUOTED_IDENTIFIER`, otherwise inserts into Identity tables may fail.

Create an admin test user by registering via the app or via curl, then assign the Administrator role via SQL:
```bash
# Get the user ID and Administrator role ID, then insert into AspNetUserRoles
docker exec balkanair-sql /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P 'Your_strong_Pass123' -No -d BalkanAir -I \
  -Q "INSERT INTO AspNetUserRoles (UserId, RoleId) SELECT u.Id, r.Id FROM AspNetUsers u CROSS JOIN AspNetRoles r WHERE u.Email='testadmin@balkanair.com' AND r.Name='Administrator';"
```

### Local Dev Credentials

- **SQL Server:** sa / Your_strong_Pass123 (from docker-compose.yml)
- **Admin test user:** testadmin@balkanair.com / Test123pass
- **Regular test user:** Register any email via the UI (e.g. james@balkanair.com / Secure1pass)

## Devin Secrets Needed

No external secrets required. All credentials are local development only.

## Test Plan (9 Core E2E Tests)

### Test 1: Home Page Smoke
- Open http://localhost:5050 (anonymous)
- Verify: hero text, 3 cards, navbar shows Login/Register only, Bootstrap 5 styling, footer

### Test 2: User Registration + Auto-Login
- Register a new user via /Account/Register
- Verify: redirected to home, navbar shows email/MyBookings/Logout, NO Admin link

### Test 3: Admin Login + Role-Based Navbar
- Log out, then log in as admin (testadmin@balkanair.com)
- Verify: navbar shows Admin link (proves `User.IsInRole("Administrator")` works)

### Test 4: Admin Dashboard + Entity Lists
- Click Admin link
- Verify: 6 count cards, 15 management buttons, Users list shows correct data

### Test 5: Access Denied for Non-Admin
- Log in as regular user, navigate directly to /Administration
- Verify: redirected to /Account/AccessDenied (not just hidden link)

### Test 6: Profile Edit + Persistence
- Click username link, edit Phone/Nationality, save
- Verify: success message, values persist after redirect

### Test 7: Booking Search with Empty Data
- Click Book a Flight
- Verify: form renders with empty dropdowns (no airports seeded), validation errors on submit, no 500

### Test 8: Logout Flow
- Click Logout
- Verify: navbar reverts to Login/Register, /Profile redirects to /Account/Login

### Test 9: Invalid Login Validation
- Enter wrong credentials
- Verify: stays on login page, shows "Invalid login attempt." error, no stack trace

## Troubleshooting

- **Port 5000 in use:** Use `--urls "http://0.0.0.0:5050"` or another free port
- **sqlcmd QUOTED_IDENTIFIER error:** Always use `-I` flag with sqlcmd for Identity table inserts
- **EF migrations not available:** Use `EnsureCreated()` as a temporary workaround; `dotnet-ef` tool version must match the .NET SDK version (e.g. 8.x for .NET 8)
- **HTTPS redirect loop:** Disable `UseHttpsRedirection()` when testing locally without a cert
- **Docker SQL Server not ready:** Wait 10-15 seconds after container start before running sqlcmd
