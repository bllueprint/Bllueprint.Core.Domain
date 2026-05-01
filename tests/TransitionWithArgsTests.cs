using FluentAssertions;

namespace Bllueprint.Core.Domain.Tests;

public class TransitionWithArgsTests : DomainTest
{
    [Fact]
    public void Invoke_WhenAllGuardsPass_ShouldExecuteActionWithArgs()
    {
        var order = new Order();
        order.Pay();

        order.Ship("TRACK-001");

        order.IsShipped.Should().BeTrue();
        order.TrackingNumber.Should().Be("TRACK-001");
        Notifications.IsEmpty.Should().BeTrue();
    }

    [Fact]
    public void Invoke_WhenFirstGuardFails_ShouldAddErrorAndNotShip()
    {
        var order = new Order(); // not paid

        order.Ship("TRACK-001");

        order.IsShipped.Should().BeFalse();
        Notifications.ValidationErrors.Should().ContainSingle()
            .Which.Message.Should().Be("Order must be paid before shipping.");
    }

    [Fact]
    public void Invoke_WhenSecondGuardFails_ShouldAddErrorAndNotShip()
    {
        var order = new Order();
        order.Pay();

        order.Ship(string.Empty);

        order.IsShipped.Should().BeFalse();
        Notifications.ValidationErrors.Should().ContainSingle()
            .Which.Message.Should().Be("Tracking number must not be empty.");
    }

    [Fact]
    public void Invoke_WhenFirstGuardFails_ShouldNotEvaluateSecondGuard()
    {
        var order = new Order(); // not paid AND empty tracking � only first guard fires

        order.Ship(string.Empty);

        Notifications.ValidationErrors.Should().ContainSingle();
    }

    [Fact]
    public void Invoke_WhenGuardFails_ShouldUseCallerNameAsTransitionName()
    {
        var order = new Order();

        order.Ship("TRACK-001"); // fails: not paid

        Notifications.All.Should().ContainSingle()
            .Which.TransitionName.Should().Be("Ship");
    }
}
