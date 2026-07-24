# ---- Build stage ----
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy only .csproj files first (better layer caching)
COPY src/NotificationManagement.API/*.csproj src/NotificationManagement.API/
COPY src/NotificationManagement.Application/*.csproj src/NotificationManagement.Application/
COPY src/NotificationManagement.Domain/*.csproj src/NotificationManagement.Domain/
COPY src/NotificationManagement.Infrastructure/*.csproj src/NotificationManagement.Infrastructure/

RUN dotnet restore "src/NotificationManagement.API/NotificationManagement.API.csproj"

# Copy the rest of the source code
COPY src/ src/

RUN dotnet publish "src/NotificationManagement.API/NotificationManagement.API.csproj" -c Release -o /app/publish --no-restore

# ---- Runtime stage ----
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "NotificationManagement.API.dll"]