using Bllueprint.Core.Domain;
using FluentAssertions;

namespace Bllueprint.Core.Domain.Tests;

public class NotificationContextTests : DomainTest
{
    [Fact]
    public void IsEmpty_WhenNoNotificationsAdded_ShouldBeTrue()
        => Notifications.IsEmpty.Should().BeTrue();

    [Fact]
    public void HasErrors_WhenNoNotificationsAdded_ShouldBeFalse()
        => Notifications.HasErrors.Should().BeFalse();

    [Fact]
    public void Add_Notification_ShouldAppearInAll()
    {
        var notification = new Notification
        {
            TransitionName = "Pay",
            Message = "Something went wrong.",
            Kind = NotificationKind.Error
        };

        Notifications.Add(notification);

        Notifications.All.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(notification);
    }

    [Fact]
    public void Add_ConvenienceOverload_ShouldDefaultToError()
    {
        Notifications.Add("Pay", "Something went wrong.");

        Notifications.All.Should().ContainSingle()
            .Which.Kind.Should().Be(NotificationKind.Error);
    }

    [Fact]
    public void Add_ConvenienceOverload_WithWarningKind_ShouldPersistKind()
    {
        Notifications.Add("CheckStock", "Low stock warning.", NotificationKind.Warning);

        Notifications.All.Should().ContainSingle()
            .Which.Kind.Should().Be(NotificationKind.Warning);
    }

    [Fact]
    public void HasErrors_AfterAddingError_ShouldBeTrue()
    {
        Notifications.Add("Pay", "Error occurred.");

        Notifications.HasErrors.Should().BeTrue();
    }

    [Fact]
    public void HasErrors_WhenOnlyWarningsAdded_ShouldBeFalse()
    {
        Notifications.Add("CheckStock", "Low stock.", NotificationKind.Warning);

        Notifications.HasErrors.Should().BeFalse();
    }

    [Fact]
    public void IsEmpty_AfterAddingAnyNotification_ShouldBeFalse()
    {
        Notifications.Add("Pay", "err");

        Notifications.IsEmpty.Should().BeFalse();
    }

    [Fact]
    public void ValidationErrors_ShouldReturnOnlyErrors()
    {
        Notifications.Add("A", "error msg", NotificationKind.Error);
        Notifications.Add("B", "warning msg", NotificationKind.Warning);

        Notifications.ValidationErrors.Should().ContainSingle()
            .Which.Message.Should().Be("error msg");
    }

    [Fact]
    public void Warnings_ShouldReturnOnlyWarnings()
    {
        Notifications.Add("A", "error msg", NotificationKind.Error);
        Notifications.Add("B", "warning msg", NotificationKind.Warning);

        Notifications.Warnings.Should().ContainSingle()
            .Which.Message.Should().Be("warning msg");
    }

    [Fact]
    public void All_ShouldPreserveInsertionOrder()
    {
        Notifications.Add("A", "first");
        Notifications.Add("B", "second");
        Notifications.Add("C", "third");

        Notifications.All.Select(n => n.Message)
            .Should().ContainInOrder("first", "second", "third");
    }
}
