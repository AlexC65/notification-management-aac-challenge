# Notification Management Challenge

Full Stack Take Home Challenge – Notification management REST API with multi-channel support (Email, SMS, Push). Built with .NET C# and clean architecture.

## Features

- **Multi-channel notifications**: Send notifications via Email, SMS, and Push in a unified way.
- **Strategy + Factory pattern**: Multiple notification strategies coexist and are resolved dynamically, making it easy to add new channels without breaking existing ones.
- **Clean Architecture**: Business logic is decoupled from infrastructure and application configuration for easier testing and maintenance.
- **JWT Authentication**: Secure endpoints using JSON Web Tokens.
- **PostgreSQL persistence**: Reliable relational storage via Docker-hosted PostgreSQL.
- **EF Core migrations**: Database schema managed through Entity Framework Core migrations.

## Badges

[![CircleCI](https://dl.circleci.com/status-badge/img/gh/AlexC65/notification-management-aac-challenge/tree/main.svg?style=svg)](https://dl.circleci.com/status-badge/redirect/gh/AlexC65/notification-management-aac-challenge/tree/main)

[![Coverage Status](https://coveralls.io/repos/github/AlexC65/notification-management-aac-challenge/badge.svg?branch=main)](https://coveralls.io/github/AlexC65/notification-management-aac-challenge?branch=main)

## Requirements

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/) (for PostgreSQL)

## Setup

### 1. Clone the repository

```bash
git clone https://github.com/your-username/notification-management-{initials}-challenge.git
cd notification-management-{initials}-challenge
```

### 2. Start PostgreSQL with Docker

```bash
docker run --name notifications-db \
  -e POSTGRES_USER=postgres \
  -e POSTGRES_PASSWORD=postgres \
  -e POSTGRES_DB=notifications_db \
  -p 5432:5432 \
  -d postgres
```

### 3. Configure JWT Secret

```bash
dotnet user-secrets set "Jwt:Secret" "your-secret-key-minimum-32-characters!" \
  --project src/NotificationManagement.API
```

### 4. Run migrations

```bash
dotnet ef database update \
  --project src/NotificationManagement.Infrastructure \
  --startup-project src/NotificationManagement.API
```

### 5. Run the API

```bash
dotnet run --project src/NotificationManagement.API
```

## API Endpoints

### Auth

| Method | Endpoint             | Description                                                       |
|--------|-----------------------|---------------------------------------------------------------------|
| POST   | `/api/auth/register` | Registers a new user account and returns a JWT token.               |
| POST   | `/api/auth/login`    | Authenticates an existing user with email/password and returns a JWT token. |

### Notifications
*All endpoints below require a valid JWT (`Authorization: Bearer <token>`).*

| Method | Endpoint                    | Description                                                                 |
|--------|------------------------------|-------------------------------------------------------------------------------|
| GET    | `/api/notifications`        | Returns a paginated list of notifications belonging to the authenticated user. Supports `page` and `pageSize` query parameters. |
| POST   | `/api/notifications`        | Creates a notification and dispatches it immediately through the specified channel. Body: `{"title", "content", "channel", "recipient"}`. |
| PUT    | `/api/notifications/{id}`   | Updates the title, content, channel, and recipient of an existing notification owned by the authenticated user. Body: `{"title", "content", "channel", "recipient"}`. |
| DELETE | `/api/notifications/{id}`   | Deletes an existing notification owned by the authenticated user.            |

## Decisions Taken

- **Strategy + Factory pattern**: Each notification channel (Email, SMS, Push) implements the `INotificationChannel` interface (Strategy pattern), exposing its own `ChannelType`. All implementations are registered independently in the DI container and injected as an `IEnumerable<INotificationChannel>` into `NotificationChannelFactory`, which builds a dictionary keyed by `ChannelType` and exposes a single `Resolve(ChannelType channel)` method (Simple Factory pattern). This avoids the common pitfall of registering multiple implementations under the same interface — where the DI container would only resolve the last one registered, silently overriding the previous strategies — while making it straightforward to add new channels in the future without touching existing code.

- **Clean Architecture**: The solution is organized into `Domain`, `Application`, `Infrastructure`, and `API` layers, keeping business logic (notification dispatch rules, DTOs, service interfaces) decoupled from infrastructure concerns (EF Core, channel implementations) and framework/configuration details (`Program.cs`, JWT setup). This separation makes the codebase easier to test, extend, and maintain.
