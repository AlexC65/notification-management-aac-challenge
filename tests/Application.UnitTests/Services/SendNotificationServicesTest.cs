using Moq;
using NotificationManagement.Application.Services;
using NotificationManagement.Domain.Entities;
using NotificationManagement.Application.Interfaces;

namespace Application.UnitTests.Services
{
    public class SendNotificationServicesTest
    {
        private readonly Mock<ISendNotification> _mockSender;
        private readonly SendNotificationServices _service;

        private readonly Guid _validUserId = Guid.NewGuid();
        private const string _validTitle = "Test Title";
        private const string _validContent = "Test Content";
        private const ChannelType _validChannel = ChannelType.Email;
        private const string _validRecipient = "user@example.com";

        public SendNotificationServicesTest()
        {
            _mockSender = new Mock<ISendNotification>();
            _service = new SendNotificationServices(_mockSender.Object);
        }

        [Fact]
        public async Task SendNotificationAsync_WhenRecipientIsValid_ShouldCallValidateOnce()
        {
            // Arrange
            _mockSender
                .Setup(s => s.ValidateAsync(_validRecipient))
                .ReturnsAsync(true);

            var notification = new Notification(
                _validUserId, _validTitle, _validContent, _validChannel, _validRecipient);

            // Act
            await _service.SendNotificationAsync(notification);

            // Assert
            _mockSender.Verify(s => s.ValidateAsync(_validRecipient), Times.Once);
        }

        [Fact]
        public async Task SendNotificationAsync_WhenValidationFails_ShouldThrowInvalidOperationException()
        {
            // Arrange
            _mockSender
                .Setup(s => s.ValidateAsync(_validRecipient))
                .ReturnsAsync(false); // simulamos que el validador rechaza

            var notification = new Notification(
                _validUserId, _validTitle, _validContent, _validChannel, _validRecipient);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.SendNotificationAsync(notification));
        }

        [Fact]
        public async Task SendNotificationAsync_WhenValidationFails_ShouldNeverSend()
        {
            // Arrange
            _mockSender
                .Setup(s => s.ValidateAsync(_validRecipient))
                .ReturnsAsync(false);

            var notification = new Notification(
                _validUserId, _validTitle, _validContent, _validChannel, _validRecipient);

            // Act
            try { await _service.SendNotificationAsync(notification); } catch { }

            // Assert — nunca debería intentar enviar si la validación falla
            _mockSender.Verify(s => s.RecordDeliveryAsync(It.IsAny<Notification>()), Times.Never);
        }
    }
}