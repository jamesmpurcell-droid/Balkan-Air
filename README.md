# Balkan-Air

> **Modernisation in progress (.NET 8).** This fork is migrating the original
> .NET Framework 4.5.2 / ASP.NET Web Forms application to .NET 8. The original
> source is preserved untouched under [`legacy/`](./legacy) and is gradually
> replaced by new projects under `src/` and `tests/` (strangler-fig approach).

## Repository layout

| Path | Contents |
|------|----------|
| `src/` | New .NET 8 projects (added per migration PR) |
| `tests/` | New .NET 8 test projects |
| `legacy/` | Original .NET Framework solution (`legacy/Balkan Air`), screenshots, AppVeyor config |
| `BalkanAir.sln` | The new .NET 8 solution |

## Build & test (.NET 8)

```bash
# requires .NET 8 SDK (pinned in global.json)
dotnet build BalkanAir.sln
dotnet test  BalkanAir.sln

# local SQL Server for later data/API work
docker compose up -d sqlserver
```

CI runs build + test on every push/PR via GitHub Actions (`.github/workflows/ci.yml`).

---

## Original project

This application simulates an online reservation system that allows users to search for the best flights available, book seats in different travel classes, receive emails and notifications for new flights and news, manage their personal profile, write comments on news and receive data via the open API. 

The system automatically sends email with callback URL (account confirmation link) to every registered user, so it is important for users to provide a **VALID** email, in order to be able to book flights later. Email with callback URL (flight confirmation link) is also send to user, when new flight is booked from him.

Check the **Developers** web page for API Overview.

[![Build status](https://ci.appveyor.com/api/projects/status/nb17l5bd48fp1h67?svg=true)](https://ci.appveyor.com/project/itplamen/balkan-air)

## Technologies

* ASP.NET Web Forms with model binding - [link](https://github.com/itplamen/Balkan-Air/blob/master/Balkan%20Air/Web/BalkanAir.Web/Administration/LegInstancesManagement.aspx)
* ASP.NET Web API - [link](https://github.com/itplamen/Balkan-Air/blob/master/Balkan%20Air/Api/BalkanAir.Api/Controllers/FlightsController.cs)
* Entity Framework Code First - [link](https://github.com/itplamen/Balkan-Air/blob/master/Balkan%20Air/Data/BalkanAir.Data.Models/Booking.cs)
* Repository pattern - [link](https://github.com/itplamen/Balkan-Air/blob/master/Balkan%20Air/Data/BalkanAir.Data/Repositories/GenericRepository.cs)
* MVP pattern - [link](https://github.com/itplamen/Balkan-Air/blob/master/Balkan%20Air/Mvp/BalkanAir.Mvp/Presenters/Administration/AirportsManagementPresenter.cs)

## Libraries

* Ninject Dependency Injector - [link](https://github.com/ninject)
* Automapper, object-object mapper - [link](https://github.com/AutoMapper/AutoMapper)
* Moq, mocking framework - [link](https://github.com/moq/moq4)
* WebFormsMvp - [link](https://github.com/webformsmvp/webformsmvp)
* MyTested.WebApi - [link](https://github.com/ivaylokenov/MyTested.WebApi) 
* ASP.NET AJAX Control Toolkit - [link](https://github.com/DevExpress/AjaxControlToolkit)
* Bootstrap
* jQuery and jQuery UI

## Screenshots

![Home page](./legacy/Screenshots/Home/01.%20Home.png)

![Home page](./legacy/Screenshots/Home/02.%20Home.png)

![Booking page](./legacy/Screenshots/Booking/01.%20Select%20Flight.png)

![Booking page](./legacy/Screenshots/Booking/03.%20Select%20Seat.png)

![API page](./legacy/Screenshots/API/02.%20API%20Overview.png)
