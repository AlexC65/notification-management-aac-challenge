using Xunit;
using Moq;
using FluentAssertions;

namespace NotificationManagement.UnitTests.Users;

public class UserServiceTests
{
    // TODO: Register (valid email/password -> success)
    // TODO: Register (duplicate email -> conflict)
    // TODO: Register (invalid email format -> validation error)
    // TODO: Register (weak password -> validation error)
    // TODO: Register (password is stored hashed, never plain text)
 
    // TODO: Login (correct credentials -> valid token)
    // TODO: Login (non-existent email -> unauthorized, generic message)
    // TODO: Login (wrong password -> unauthorized)
    // TODO: Login (token contains expected claims: user id, expiration)
}