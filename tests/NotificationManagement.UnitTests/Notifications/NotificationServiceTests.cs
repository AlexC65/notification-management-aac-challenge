using Xunit;
using Moq;
using FluentAssertions;

namespace NotificationManagement.UnitTests.Notifications;

public class NotificationServiceTests
{
    // TODO: CreateAsync (valid data -> creates notification and triggers send)
    // TODO: UpdateAsync (existing notification owned by user -> updates)
    // TODO: UpdateAsync (notification owned by another user -> unauthorized/forbidden)
    // TODO: DeleteAsync (existing notification -> deletes)
    // TODO: GetMyNotificationsAsync (returns only current user's notifications, paginated)
}

public class NotificationChannelFactoryTests
{
    // TODO: Resolve(Email) -> returns EmailChannel instance
    // TODO: Resolve(Sms) -> returns SmsChannel instance
    // TODO: Resolve(Push) -> returns PushChannel instance
    // TODO: Resolve(unsupported channel) -> throws
}