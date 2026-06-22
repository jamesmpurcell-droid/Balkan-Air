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
- 4 aircraft (A320neo, A321neo, 737 MAX 8, E190-E2), 3 manufacturers (Airbus, Boeing, Embraer)
- 3 news categories, 5 news articles
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

#### Test 1: Flights Page with Route Info + Pagination
- Navigate to /Flights
- Verify: "Showing 84 flights" text visible
- Verify: Route column shows "XXX → YYY" format (e.g. "SOF → LHR"), NOT numeric IDs
- Verify: Duration column present (e.g. "3:20", "1:00")
- Verify: Pagination nav at bottom with pages 1-5 (84 flights / 20 per page)
- Click page 2 → verify different flights appear (different dates)

#### Test 2: Flight Details Page
- Click "Details" on any flight from /Flights
- Verify: Route Information card (Route, From airport name, To airport name, Distance in km)
- Verify: Schedule & Pricing card (Departure, Arrival, Duration in "Xh Ym" format, Price, Status badge)
- Verify: Aircraft section (manufacturer + model + total seats)
- Verify: Green "Book This Flight" button links to /Booking/Confirm?legInstanceId=X

### Phase 2: Auth Flows + Booking

#### Test 3: Full Booking Flow (Logged In)
1. Log in as james@balkanair.com or testadmin@balkanair.com
2. From Flight Details, click "Book This Flight"
3. Verify Confirm page shows: Route (SOF → LHR), From (airport name), To (airport name), Departure, Arrival, Duration, Price (green)
4. Verify Booking Details form: Travel Class dropdown, Row input, Seat number input (placeholder "A-F")
5. Submit with Row=1, Seat=A
6. Verify Confirmation page: confirmation code (6 chars), route "SOF → LHR", airport names, departure/arrival
7. Click "View My Bookings" → verify table has Route and Departure columns

**Known issue:** Booking TotalPrice may show ¤0.00 — this is a pre-existing calculation issue, not a view bug.

#### Test 4: Admin Login + Dashboard
- Log in as testadmin@balkanair.com
- Verify: navbar shows Admin link, navigate to /Administration
- Verify entity counts: Aircraft=4, Airports=12+, Flights=12, News=5, Users=2

#### Test 5: Role-Based Access (Non-Admin Blocked)
- Log in as james@balkanair.com (User role)
- Navigate directly to /Administration
- Verify: redirected to Access Denied page ("You do not have permission")

### Phase 3: Admin CRUD Forms

#### Test 6: Aircraft CRUD
1. From /Administration, click Aircraft
2. Verify list shows 4 seeded aircraft with Model, Seats, Manufacturer columns
3. Verify "+ Add Aircraft" button exists
4. Click "+ Add Aircraft" → verify Manufacturer dropdown populated (Airbus, Boeing, Embraer)
5. Fill Model="A380", Manufacturer=Airbus, Seats=555 → submit
6. Verify success alert "Aircraft 'A380' created." + row in table
7. Click "Edit" on A380 → change seats → save → verify "Aircraft 'A380' updated."

#### Test 7: Country CRUD + Abbreviation Validation
1. From /Administration, click Countries
2. Verify 10+ seeded countries visible with 2-char abbreviations
3. Click Edit on any country → verify label says "ISO Code (2 chars)"
4. Verify input has maxLength=2 (browser prevents typing >2 chars)
5. Try typing 3+ chars → confirm only 2 chars accepted

**Bug fix context:** Previously StringLength(5) allowed 3+ chars which caused SQL truncation error against MaxLength(2) DB column. Now fixed to StringLength(2).

#### Test 8: Airport CRUD
1. Click Airports from dashboard
2. Verify 12+ seeded airports with IATA codes and country names
3. Click "+ Add Airport"
4. Verify Country dropdown populated with seeded countries
5. Create airport (e.g. "Dublin Airport", IATA "DUB", Country=Ireland)
6. Verify success banner and new row in list

#### Test 9: News CRUD
1. Click News from dashboard
2. Verify 5 seeded articles visible
3. Click "+ Add Article" → fill Title, select Category (Routes/Fleet/Company), add Content
4. Submit → verify success banner + article in list
5. Click Edit → change title → save → verify update in list

### Phase 4: Validation & Edge Cases

#### Test 10: Invalid Login
- Enter wrong credentials → verify "Invalid login attempt." error, no stack trace

#### Test 11: Logout Flow
- Click Logout → verify navbar reverts to Login/Register, /Profile redirects to login

## Troubleshooting

- **Port 5050 in use:** Kill existing process: `kill $(lsof -t -i:5050)` then restart
- **App not reflecting code changes:** Kill old dotnet process and restart from the correct branch
- **sqlcmd QUOTED_IDENTIFIER error:** Always use `-I` flag for Identity table inserts
- **Database schema conflicts (SQL 2714 "object already exists"):** Drop DB and restart app
- **Seed data not appearing:** Check /tmp/webapp.log for "Database already seeded" or "Seeding database..." message
- **Date input not accepting typed values in Chrome:** Use JS `document.querySelector('input[type="date"]').value = 'YYYY-MM-DD'`
- **HTTPS redirect loop:** Disable `UseHttpsRedirection()` when testing locally without a cert
- **Docker SQL Server not ready:** Wait 10-15 seconds after container start before running sqlcmd
- **EF migration failures after schema changes:** Drop the database and let the app re-create and re-seed on restart
- **Booking total shows ¤0.00:** Pre-existing issue with TotalPrice calculation in Booking entity, not a view bug
- **Currency symbol shows ¤ instead of €/£:** App uses InvariantCulture for formatting; cosmetic only
