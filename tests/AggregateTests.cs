using FluentAssertions;

namespace Bllueprint.Core.Domain.Tests;

public class AggregateTests : DomainTest
{
    [Fact]
    public void AddViolationMessage_ShouldAddErrorToCurrentContext()
    {
        var order = new Order();

        order.RaiseViolation("Something is invalid.");

        Notifications.ValidationErrors.Should().ContainSingle()
            .Which.Message.Should().Be("Something is invalid.");
    }

    [Fact]
    public void AddViolationMessage_KindShouldAlwaysBeError()
    {
        var order = new Order();
        order.RaiseViolation("msg");

        Notifications.All.Should().ContainSingle()
            .Which.Kind.Should().Be(NotificationKind.Error);
    }

    [Fact]
    public void AddViolationMessage_ShouldUseCallerMemberNameAsTransitionName()
    {
        var order = new Order();
        order.RaiseViolation("msg");

        Notifications.All.Should().ContainSingle()
            .Which.TransitionName.Should().Be("RaiseViolation");
    }
}
