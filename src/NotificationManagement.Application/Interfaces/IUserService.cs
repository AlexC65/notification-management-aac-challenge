using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NotificationManagement.Application.DTOs.Auth;

namespace NotificationManagement.Application.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="dto">The registration data transfer object.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<string> RegisterAsync(RegisterDto dto, CancellationToken ct = default);

        /// <summary>
        /// Authenticates a user and generates a JWT token.
        /// </summary>
        /// <param name="dto">The login data transfer object.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<string> LoginAsync(LoginDto dto, CancellationToken ct = default);
    }
}
