# Notification Management Challenge

Full Stack Take Home Challenge – Notification management REST API with multi-channel support (Email, SMS, Push). Built with .NET C# and clean architecture.

## Decisions Taken

- I decided to implement the Strategy pattern together with a Simple Factory pattern so multiple notification strategies can coexist and be resolved correctly in `Program.cs`, avoiding the issue where registering one strategy overrides the previous one, while also making it easier to add new notification types in the future.
- I follow Clean Architecture principles to keep the business logic decoupled from the application configuration and make the solution easier to extend and maintain.
