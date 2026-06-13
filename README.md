# Notification Management Challenge

Full Stack Take Home Challenge – Notification management REST API with multi-channel support (Email, SMS, Push). Built with .NET C# and clean architecture.

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

## Decisions Taken

- I decided to implement the Strategy pattern together with a Simple Factory pattern so multiple notification strategies can coexist and be resolved correctly in `Program.cs`, avoiding the issue where registering one strategy overrides the previous one, while also making it easier to add new notification types in the future.
- I follow Clean Architecture principles to keep the business logic decoupled from the application configuration and make the solution easier to extend and maintain.
