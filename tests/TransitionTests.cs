using FluentAssertions;

namespace Bllueprint.Core.Domain.Tests;

public class TransitionTests : DomainTest
{
    [Fact]
    public void Invoke_WhenGuardPasses_ShouldExecuteAction()
    {
        var order = new Order();

        order.Pay();

        order.IsPaid.Should().BeTrue();
        Notifications.IsEmpty.Should().BeTrue();
    }

    [Fact]
    public void Invoke_WhenGuardFails_ShouldNotExecuteAction()
    {
        var order = new Order();
        order.Pay();
        order.Pay(); // fails � already paid

        order.IsPaid.Should().BeTrue(); // still paid, not toggled
        Notifications.HasErrors.Should().BeTrue();
    }

    [Fact]
    public void Invoke_WhenGuardFails_ShouldAddCorrectErrorMessage()
    {
        var order = new Order();
        order.Pay();
        order.Pay();

        Notifications.ValidationErrors.Should().ContainSingle()
            .Which.Message.Should().Be("Order is already paid.");
    }

    [Fact]
    public void Invoke_WhenGuardFails_ShouldUseCallerNameAsTransitionName()
    {
        var order = new Order();
        order.Pay();
        order.Pay();

        Notifications.All.Should().ContainSingle()
            .Which.TransitionName.Should().Be("Pay");
    }

    [Fact]
    public void Invoke_WhenFirstGuardFails_ShouldNotEvaluateSubsequentGuards()
    {
        var order = new Order();
        order.Pay();
        order.Pay(); // only "already paid" fires, not "cannot pay cancelled"

        Notifications.ValidationErrors.Should().ContainSingle();
    }

    [Fact]
    public void Invoke_ActionBeforeGuard_ShouldFireOnlyAfterGuardPasses()
    {
        bool fired = false;

        ITransition<Order> transition = new TransitionBuilder<Order>()
            .Do(_ => fired = true)
            .Requires(_ => true, "always passes")
            .Create();

        transition.Invoke(new Order());

        fired.Should().BeTrue();
    }

    [Fact]
    public void Invoke_ActionBeforeFailingGuard_ShouldNotFire()
    {
        bool fired = false;

        ITransition<Order> transition = new TransitionBuilder<Order>()
            .Do(_ => fired = true)
            .Requires(_ => false, "always fails")
            .Create();

        transition.Invoke(new Order());

        fired.Should().BeFalse();
    }

    [Fact]
    public void Invoke_MultipleActionsBeforeGuard_AllFireWhenGuardPasses()
    {
        var log = new List<string>();

        ITransition<Order> transition = new TransitionBuilder<Order>()
            .Do(_ => log.Add("first"))
            .Do(_ => log.Add("second"))
            .Requires(_ => true, "passes")
            .Create();

        transition.Invoke(new Order());

        log.Should().ContainInOrder("first", "second");
    }

    [Fact]
    public void Invoke_NoSteps_ShouldCompleteWithoutError()
    {
        ITransition<Order> transition = new TransitionBuilder<Order>().Create();

        Action act = () => transition.Invoke(new Order());

        act.Should().NotThrow();
        Notifications.IsEmpty.Should().BeTrue();
    }
}
