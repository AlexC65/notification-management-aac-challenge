using Microsoft.AspNetCore.Mvc;
using NotificationManagement.Application.Interfaces;
using NotificationManagement.Application.DTOs.Auth;
using NotificationManagement.Application.Services;
using System.ComponentModel.DataAnnotations;

namespace NotificationManagement.API.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IUserService _userServices;

    public AuthController(IUserService userService)
    {
        _userServices = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct)
    {
        var dto = new RegisterDto(request.Name, request.Email, request.Password);
        var token = await _userServices.RegisterAsync(dto, ct);
        return Ok(new { token });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var dto = new LoginDto(request.Email, request.Password);
        var token = await _userServices.LoginAsync(dto, ct);
        return Ok(new { token });
    }
}

// Request records
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
public record LoginRequest(
    [Required]
    [EmailAddress]
    [MaxLength(256)]
    string Email,
    
    [Required]
    [MinLength(8)]
    string Password);