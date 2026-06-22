using NotificationManagement.Infrastructure.Channels;

namespace Application.UnitTests.Infrastructure
{
    public class EmailNotificationServiceTests
    {
        private readonly EmailNotificationService _service = new();

        [Fact]
        public async Task ValidateAsync_WithValidEmail_ShouldReturnTrue()
        {
            var result = await _service.ValidateAsync("user@example.com");

            Assert.True(result);
        }

        [Theory]
        [InlineData("userexample.com")]   // sin @
        [InlineData("user#example.com")]  // con # en vez de @
        // [InlineData("@example.com")]      // sin usuario
        // [InlineData("user@")]             // sin dominio
        public async Task ValidateAsync_WithInvalidEmail_ShouldThrowArgumentException(string invalidEmail)
        {
            await Assert.ThrowsAsync<ArgumentException>(
                () => _service.ValidateAsync(invalidEmail));
        }
    }
}
