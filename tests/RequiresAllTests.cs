using FluentAssertions;

namespace Bllueprint.Core.Domain.Tests;

public class RequiresAllTests : DomainTest
{
    [Fact]
    public void Invoke_WhenAllGuardsPass_ShouldExecuteAction()
    {
        var order = new Order(); // fresh: not paid, not shipped

        order.Cancel();

        order.IsCancelled.Should().BeTrue();
        Notifications.IsEmpty.Should().BeTrue();
    }

    [Fact]
    public void Invoke_WhenOneGuardFails_ShouldReportThatFailure()
    {
        var order = new Order();
        order.Pay();

        order.Cancel();

        order.IsCancelled.Should().BeFalse();
        Notifications.ValidationErrors.Should().ContainSingle()
            .Which.Message.Should().Be("Cannot cancel a paid order.");
    }

    [Fact]
    public void Invoke_WhenMultipleGuardsFail_ShouldReportAllFailures()
    {
        var order = new Order();
        order.Pay();
        order.ForceShip(); // both guards now fail

        order.Cancel();

        Notifications.ValidationErrors.Should().HaveCount(2)
            .And.Satisfy(
                e => e.Message == "Cannot cancel a paid order.",
                e => e.Message == "Cannot cancel a shipped order.");
    }

    [Fact]
    public void Invoke_WhenMultipleGuardsFail_ShouldNotExecuteAction()
    {
        var order = new Order();
        order.Pay();
        order.ForceShip();

        order.Cancel();

        order.IsCancelled.Should().BeFalse();
    }
}
