
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationManagement.Infrastructure.Persistence;

namespace NotificationManagement.Infrastructure;

public  static class DependencyInjection
{
    public static IServiceCollection addInfrastructure( 
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // EF Core + PostgreSQL
        services.AddDbContext<AppDbContext>(options => 
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
        );

        return services;
    }
}