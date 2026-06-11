using NotificationManagement.Domain.Entities;

namespace NotificationManagement.Application.Interfaces
{
    /// <summary>
    /// Responsible for generating JWT access tokens for authenticated users.
    /// </summary>
    public interface IJwtTokenGenerator
    {
        /// <summary>
        /// Generates a JWT for the specified user information.
        /// </summary>
        /// <param name="user">The user for whom to generate a token.</param>
        /// <returns>The generated JWT as a string.</returns>
        string GenerateToken(User user);
    }
}
