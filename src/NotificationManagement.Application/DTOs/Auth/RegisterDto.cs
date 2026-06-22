
namespace NotificationManagement.Application.DTOs.Auth;

public sealed record RegisterDto(
    string UserName,
    string Email,
    string Password
);


