# RoyalVilla

A villa discovery and management web application built with ASP.NET Core 10, Entity Framework Core 10, and SQL Server.

The solution includes a REST API and an ASP.NET Core MVC website that consumes it.

## Table of Contents

- [Tech Stack](#tech-stack)
- [Features](#features)
- [Screenshots](#screenshots)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Setup](#setup)
  - [Demo Credentials](#demo-credentials)
- [API Endpoints](#api-endpoints)
- [Project Structure](#project-structure)
- [Deployment](#deployment)

## Tech Stack

| Layer | Technology |
|---|---|
| Website | ASP.NET Core 10 MVC, Razor Views |
| API | ASP.NET Core 10 Web API, REST, OpenAPI, Scalar |
| Business logic | C# services, DTOs, AutoMapper |
| Data access | Entity Framework Core 10, SQL Server |
| Authentication | ASP.NET Core Identity, JWT Bearer tokens |
| Frontend | Bootstrap 5, HTML, CSS, JavaScript |

## Features

- Villa catalog - browse API-backed villa listings and view villa details.
- Search - filter the villas returned by the API by name or description.
- Amenities - display amenities associated with a villa.
- Villa management - Admin users can create, edit, and delete villas.
- Authentication - register a regular user or sign in with email and password.
- Role-based access - new accounts receive the `User` role; villa write endpoints require `Admin`.
- API client - the MVC website uses `IHttpClientFactory`, a dedicated API service, and a configuration-based API URL.
- Resilient UI - loading failures and empty results show helpful states; forms include validation and feedback.
- Responsive design - layouts adapt to mobile, tablet, and desktop screens.

## Screenshots

Screenshots are not included in the repository yet.

## Getting Started

### Prerequisites

- .NET 10 SDK
- SQL Server, SQL Server LocalDB, or SQL Server running in Docker
- The `dotnet-ef` tool (for applying database migrations)

Install the EF Core command-line tool if needed:

```bash
dotnet tool install --global dotnet-ef --version 10.0.12
```

### Setup

1. Clone the repository:

   ```bash
   git clone https://github.com/Ahm3dHegazy/RoyalVilla.git
   cd RoyalVilla
   ```

2. Start SQL Server. For Docker:

   ```bash
   docker run --name royalvilla-sql \
     -e ACCEPT_EULA=Y \
     -e MSSQL_SA_PASSWORD='Use-a-strong-password-here!' \
     -p 1433:1433 \
     -d mcr.microsoft.com/mssql/server:2022-latest
   ```

   On Windows, you can use SQL Server LocalDB instead.

3. Configure the API secrets from the repository root. These values are stored with .NET User Secrets and should not be committed:

   ```bash
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=RoyalVilla;User Id=sa;Password=Use-a-strong-password-here!;TrustServerCertificate=True;Encrypt=False" --project RoyalVilla_API
   dotnet user-secrets set "Jwt:Key" "replace-with-a-random-secret-of-at-least-32-characters" --project RoyalVilla_API
   dotnet user-secrets set "SeedAdmin:Email" "admin@example.com" --project RoyalVilla_API
   dotnet user-secrets set "SeedAdmin:Password" "Use-a-strong-admin-password-here!" --project RoyalVilla_API
   ```

   For LocalDB, use a connection string such as:

   ```text
   Server=(localdb)\MSSQLLocalDB;Database=RoyalVilla;Integrated Security=SSPI;TrustServerCertificate=True
   ```

   Replace the example passwords and JWT key with your own values. The API project already has a User Secrets ID configured.

4. Apply the database migrations:

   ```bash
   dotnet ef database update \
     --project RoyalVilla.Data \
     --startup-project RoyalVilla_API
   ```

5. Start the API in one terminal:

   ```bash
   dotnet run --project RoyalVilla_API
   ```

   The HTTP launch profile listens at `http://localhost:5244`. In Development, Scalar is available at `http://localhost:5244/Scalar`.

6. Start the MVC website in another terminal:

   ```bash
   dotnet run --project RoyalVillaWeb
   ```

   The website listens at `http://localhost:5274` and uses `Api:BaseUrl` from `RoyalVillaWeb/appsettings.json` to connect to the API.

### Demo Credentials

There are no hardcoded demo credentials. The API creates the Admin account configured by `SeedAdmin:Email` and `SeedAdmin:Password` when it starts, if that email does not already exist. You can also create a regular account from the website's **Create account** page; regular users can sign in and browse but cannot manage villas.

## API Endpoints

| Method | Endpoint | Access | Description |
|---|---|---|---|
| `GET` | `/api/Villa` | Public | List villas |
| `GET` | `/api/Villa/{id}` | Public | Get villa details |
| `POST` | `/api/Villa` | Admin | Create a villa |
| `PUT` | `/api/Villa/{id}` | Admin | Update a villa |
| `DELETE` | `/api/Villa/{id}` | Admin | Delete a villa |
| `GET` | `/api/VillaAmenities?villaId={id}` | Public | List a villa's amenities |
| `GET` | `/api/VillaAmenities/{id}` | Public | Get an amenity |
| `POST` | `/api/Auth/Register` | Public | Register a regular user |
| `POST` | `/api/Auth/Login` | Public | Sign in and receive a JWT |

Villa and amenity API responses use the `ApiResponse<T>` envelope. Authentication returns a JWT token used as a Bearer token for protected endpoints.

## Project Structure

```text
RoyalVilla/
├── RoyalVillaWeb/                 - ASP.NET Core MVC website
│   ├── Controllers/               - MVC request handlers
│   ├── Models/                    - API DTOs and view models
│   ├── Services/                  - Typed API client
│   ├── Views/                     - Razor pages and shared components
│   └── wwwroot/                   - CSS, JavaScript, and frontend libraries
├── RoyalVilla_API/                - REST API, authentication, and OpenAPI setup
├── RoyalVilla.Business/           - Domain models, DTOs, services, and interfaces
├── RoyalVilla.Data/               - DbContext, repositories, configurations, migrations
└── RoyalVilla.slnx                - .NET solution
```

## Deployment

Configure the API's production connection string, JWT signing key, and optional Admin seed credentials through your hosting provider's secret or environment-variable settings. Set `Api:BaseUrl` in the MVC application's production configuration to the deployed API URL.

Publish the API and website as separate ASP.NET Core applications:

```bash
dotnet publish RoyalVilla_API -c Release -o ./publish/api
dotnet publish RoyalVillaWeb -c Release -o ./publish/web
```

Run database migrations against the production database as part of deployment. Keep signing keys, database credentials, and Admin passwords out of source control.
