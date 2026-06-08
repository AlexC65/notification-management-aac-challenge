using NotificationManagement.Domain.Entities;
using NotificationManagement.Domain.Enums;

namespace Application.UnitTests.Domain
{
    public class NotificationTests
    {
        private readonly Guid _validUserId = Guid.NewGuid();
        private const string _validTitle = "Test Title";
        private const string _validContent = "Test Content";
        private const ChannelType _validChannel = ChannelType.Email;
        private const string _validRecipient = "user@example.com";

        [Fact]
        public void Constructor_WithValidData_ShouldCreateNotification()
        {
            var notification = new Notification(
                _validUserId, _validTitle, _validContent, _validChannel, _validRecipient);

            Assert.Equal(_validUserId, notification.UserId);
            Assert.Equal(_validTitle, notification.Title);
            Assert.Equal(_validContent, notification.Content);
            Assert.Equal(_validChannel, notification.Channel);
            Assert.Equal(_validRecipient, notification.Recipient);
            Assert.NotEqual(Guid.Empty, notification.Id);
            Assert.True(notification.CreatedDate <= DateTime.UtcNow);
        }

        [Fact]
        public void Constructor_WithEmptyUserId_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                new Notification(
                    Guid.Empty, _validTitle, _validContent, _validChannel, _validRecipient));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_WithInvalidTitle_ShouldThrowArgumentException(string invalidTitle)
        {
            Assert.Throws<ArgumentException>(() =>
                new Notification(
                    _validUserId, invalidTitle, _validContent, _validChannel, _validRecipient));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_WithInvalidContent_ShouldThrowArgumentException(string invalidContent)
        {
            Assert.Throws<ArgumentException>(() =>
                new Notification(
                    _validUserId, _validTitle, invalidContent, _validChannel, _validRecipient));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_WithInvalidRecipient_ShouldThrowArgumentException(string invalidRecipient)
        {
            Assert.Throws<ArgumentException>(() =>
                new Notification(
                    _validUserId, _validTitle, _validContent, _validChannel, invalidRecipient));
        }

        [Fact]
        public void Constructor_TwoInstances_ShouldHaveDifferentIds()
        {
            var n1 = new Notification(_validUserId, _validTitle, _validContent, _validChannel, _validRecipient);
            var n2 = new Notification(_validUserId, _validTitle, _validContent, _validChannel, _validRecipient);

            Assert.NotEqual(n1.Id, n2.Id);
        }
    }
}
