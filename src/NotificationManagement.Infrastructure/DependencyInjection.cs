
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationManagement.Application.Interfaces;
using NotificationManagement.Domain.Interfaces;
using NotificationManagement.Infrastructure.Auth;
using NotificationManagement.Infrastructure.Persistence;
using NotificationManagement.Infrastructure.Repositories;

namespace NotificationManagement.Infrastructure;
/// <summary>
/// Provides extension methods for registering infrastructure services.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection addInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // EF Core + PostgreSQL
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
        );
        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        //JWT
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();


        return services;
    }
}