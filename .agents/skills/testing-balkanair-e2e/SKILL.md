---
name: testing-balkanair-e2e
description: End-to-end browser testing for the Balkan Air .NET 8 web app. Use when verifying auth flows, admin dashboard, booking search, CRUD forms, seed data, and UI after code changes.
---

# Balkan Air E2E Testing

## Prerequisites

- .NET 8 SDK installed
- Docker running (for SQL Server)
- Chrome browser available

## Local Environment Setup

### 1. Start SQL Server

```bash
docker start balkanair-sql 2>/dev/null || docker run -d --name balkanair-sql \
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

### 2. Start the Web App

```bash
cd /home/ubuntu/repos/Balkan-Air/src/BalkanAir.Web
nohup dotnet run --urls "http://0.0.0.0:5050" > /tmp/webapp.log 2>&1 &
```

The app should be accessible at `http://localhost:5050`. Check startup:
```bash
grep -E "(Seeding|seeded|Now listening|error)" /tmp/webapp.log
```

### 3. Seed Data (Automatic)

The app auto-seeds on startup via `SeedData.InitializeAsync()`:
- 10 countries, 12 airports (SOF hub-and-spoke), 12 routes, 12 flights, 84 leg instances (7 days from startup)
- 4 aircraft, 3 news categories, 4 news articles
- 2 roles (User, Administrator), 2 test users

If the database already exists but is empty (or has conflicting schema), drop it and restart:
```bash
docker exec balkanair-sql /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P 'Your_strong_Pass123' -No \
  -Q "DROP DATABASE IF EXISTS BalkanAir;"
# Then restart the app
```

### 4. Test Users (Auto-Seeded)

| User | Password | Role |
|------|----------|------|
| testadmin@balkanair.com | Test123pass | Administrator |
| james@balkanair.com | Secure1pass | User |

If not seeded, register via the UI and assign admin role:
```bash
docker exec balkanair-sql /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P 'Your_strong_Pass123' -No -d BalkanAir -I \
  -Q "INSERT INTO AspNetUserRoles (UserId, RoleId) SELECT u.Id, r.Id FROM AspNetUsers u CROSS JOIN AspNetRoles r WHERE u.Email='testadmin@balkanair.com' AND r.Name='Administrator';"
```

### Local Dev Credentials

- **SQL Server:** sa / Your_strong_Pass123 (from docker-compose.yml)
- **Admin test user:** testadmin@balkanair.com / Test123pass
- **Regular test user:** james@balkanair.com / Secure1pass

## Devin Secrets Needed

No external secrets required. All credentials are local development only.

## Test Plan

### Phase 1: Public Pages (Anonymous)

#### Test 1: Home Page Smoke
- Open http://localhost:5050 (anonymous)
- Verify: hero text, 3 cards, navbar shows Login/Register only, Bootstrap 5, footer

#### Test 2: Flights Page with Seed Data
- Navigate to /Flights
- Verify: 84 leg instances visible (IDs 1-84), each shows departure/arrival datetimes + price

#### Test 3: Booking Search Dropdowns
- Navigate to /Booking/Search
- Verify: "From" and "To" dropdowns each contain 12+ airports from seed data

### Phase 2: Auth Flows

#### Test 4: Admin Login + Dashboard
- Log in as testadmin@balkanair.com
- Verify: navbar shows Admin link, navigate to /Administration
- Verify entity counts: Aircraft=4, Airports=12, Flights=12, News=4, Users=2

#### Test 5: Role-Based Access (Non-Admin Blocked)
- Log in as james@balkanair.com (User role)
- Navigate directly to /Administration
- Verify: redirected to Access Denied page ("You do not have permission")

### Phase 3: Admin CRUD Forms

#### Test 6: Country CRUD
1. From /Administration, click Countries
2. Verify 10 seeded countries visible
3. Click "+ Add Country" → submit empty → verify validation errors
4. Fill Name="Ireland", Abbreviation="IE" → submit → verify success banner + row in list
5. Click Edit on Ireland → modify abbreviation → save → verify update persists

**Known issue:** Country abbreviation DB column is MaxLength(2) but view model allows 2-5 chars. Use 2-char ISO codes only.

#### Test 7: Airport CRUD
1. Click Airports from dashboard
2. Verify 12 seeded airports with IATA codes and country names
3. Click "+ Add Airport"
4. Verify Country dropdown populated with seeded countries
5. Create airport (e.g. "Dublin Airport", IATA "DUB", Country=Ireland)
6. Verify success banner and new row in list

#### Test 8: News CRUD
1. Click News from dashboard
2. Verify 4 seeded articles visible
3. Click "+ Add Article" → fill Title, select Category (Routes/Fleet/Company), add Content
4. Submit → verify success banner + article in list
5. Click Edit → change title → save → verify update in list

### Phase 4: Booking Flow

#### Test 9: Booking Search End-to-End
1. Navigate to /Booking/Search (can be anonymous or logged in)
2. Select origin: Sofia Airport
3. Select destination: London Heathrow (or any seeded route pair)
4. Set date within 7 days of app startup (seed generates leg instances for next 7 days)
5. Click Search
6. Verify: results table shows matching flight(s) with Route, Departure, Arrival, Status=Available, Book button

**Tip:** If HTML date input doesn't accept typed values, use JavaScript:
```javascript
document.querySelector('input[name="DepartureDate"]').value = '2026-06-25';
```

### Phase 5: Validation & Edge Cases

#### Test 10: Invalid Login
- Enter wrong credentials → verify "Invalid login attempt." error, no stack trace

#### Test 11: Logout Flow
- Click Logout → verify navbar reverts to Login/Register, /Profile redirects to login

## Troubleshooting

- **Port 5000 in use:** Use `--urls "http://0.0.0.0:5050"` or another free port
- **sqlcmd QUOTED_IDENTIFIER error:** Always use `-I` flag for Identity table inserts
- **Database schema conflicts (SQL 2714 "object already exists"):** Drop DB and restart app
- **Seed data not appearing:** Check /tmp/webapp.log for "Database already seeded" or "Seeding database..." message. If neither appears, the app may not have started correctly
- **Date input not accepting typed values in Chrome:** Use JS `document.querySelector('input[type="date"]').value = 'YYYY-MM-DD'` then click Search
- **HTTPS redirect loop:** Disable `UseHttpsRedirection()` when testing locally without a cert
- **Docker SQL Server not ready:** Wait 10-15 seconds after container start before running sqlcmd
- **EF migration failures after schema changes:** Drop the database and let the app re-create and re-seed on restart
- **Country abbreviation > 2 chars causes SQL error:** This is a known validation mismatch between view model (allows 2-5) and DB (max 2). Use 2-char codes.
