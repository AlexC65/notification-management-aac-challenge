using Microsoft.AspNetCore.Mvc;
using NotificationManagement.Application.Interfaces;
using NotificationManagement.Application.DTOs.Auth;
using NotificationManagement.Application.Services;
using System.ComponentModel.DataAnnotations;

namespace NotificationManagement.API.Controllers;

/// <summary>
/// Registration and authentication endpoints. Issues JWT tokens used by the rest of the API.
/// </summary>
[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IUserService _userServices;

    public AuthController(IUserService userService)
    {
        _userServices = userService;
    }

    // ── POST /api/auth/register ────────────────────────────────────────────

    /// <summary>
    /// Registers a new user account.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/auth/register
    ///     {
    ///        "name": "Jane Doe",
    ///        "email": "jane.doe@example.com",
    ///        "password": "S3cur3Pass!"
    ///     }
    ///
    /// </remarks>
    /// <param name="request">Name, email, and password for the new account.</param>
    /// <param name="ct">Token to cancel the operation.</param>
    /// <response code="200">User registered successfully. Returns a JWT token.</response>
    /// <response code="400">The request payload is invalid (missing/invalid fields).</response>
    /// <response code="409">A user with the given email already exists.</response>
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct)
    {
        var dto = new Application.DTOs.Auth.RegisterRequest(request.Name, request.Email, request.Password);
        var token = await _userServices.RegisterAsync(dto, ct);
        return Ok(new { token });
    }

    // ── POST /api/auth/login ────────────────────────────────────────────────

    /// <summary>
    /// Authenticates an existing user and issues a JWT token.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/auth/login
    ///     {
    ///        "email": "jane.doe@example.com",
    ///        "password": "S3cur3Pass!"
    ///     }
    ///
    /// </remarks>
    /// <param name="request">Email and password of the account to authenticate.</param>
    /// <param name="ct">Token to cancel the operation.</param>
    /// <response code="200">Login successful. Returns a JWT token.</response>
    /// <response code="400">The request payload is invalid (missing/invalid fields).</response>
    /// <response code="401">Email or password is incorrect.</response>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var dto = new Application.DTOs.Auth.LoginRequest(request.Email, request.Password);
        var token = await _userServices.LoginAsync(dto, ct);
        return Ok(new { token });
    }
}

/// <summary>
/// Payload required to register a new user.
/// </summary>
public record RegisterRequest(
    [Required]
    [MaxLength(100)]
    string Name,

    [Required]
    [EmailAddress]
    [MaxLength(256)]
    string Email,

    [Required]
    [MinLength(8)]
    string Password);


/// <summary>
/// Payload required to authenticate an existing user.
/// </summary>
public record LoginRequest(
    [Required]
    [EmailAddress]
    [MaxLength(256)]
    string Email,
    
    [Required]
    [MinLength(8)]
    string Password);