
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using NotificationManagement.Application.DTOs.Auth;
using NotificationManagement.Application.Services;

namespace NotificationManagement.API.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly UserService _userServices;

    public AuthController(UserService userService)
    {
        _userServices = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct)
    {
        var dto = new RegisterDto(request.Name, request.Email, request.Password);
        var token = await _userServices.RegisterAsync(dto,ct);
        return Ok(new { token });
    }

    // Request records
    public record RegisterRequest(string Name, string Email, string Password);
    public record LoginRequest(string Email, string Password);
}