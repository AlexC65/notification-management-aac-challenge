using Microsoft.Extensions.DependencyInjection;
using NotificationManagement.Application.Services;

namespace NotificationManagement.Application
{
    /// <summary>
    /// Provides extension methods for registering services
    /// from the Application layer.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Registers the Application layer services in the dependency injection container.
        /// </summary>
        /// <param name="services">
        /// The <see cref="IServiceCollection"/> used to register application services.
        /// </param>
        /// <returns>
        /// The updated <see cref="IServiceCollection"/> instance.
        /// </returns>
        public static IServiceCollection AddApplication(IServiceCollection services)
        {
            // Register application services, handlers, validators, etc.
            // e.g. services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<UserService>();

            return services;
        }
    }
}
