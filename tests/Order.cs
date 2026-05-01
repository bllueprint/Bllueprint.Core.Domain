namespace Bllueprint.Core.Domain.Tests;

sealed class Order : Aggregate<Order>
{
    public bool IsPaid { get; private set; }
    public bool IsShipped { get; private set; }
    public bool IsCancelled { get; private set; }
    public string? TrackingNumber { get; private set; }

    private static readonly ITransition<Order> _pay =
        GetTransitionBuilder()
            .Requires(o => !o.IsPaid, "Order is already paid.")
            .Requires(o => !o.IsCancelled, "Cannot pay a cancelled order.")
            .Do(o => o.IsPaid = true)
            .Create();

    private static readonly ITransition<Order> _cancel =
        GetTransitionBuilder()
            .RequiresAll(
                (o => !o.IsPaid, "Cannot cancel a paid order."),
                (o => !o.IsShipped, "Cannot cancel a shipped order."))
            .Do(o => o.IsCancelled = true)
            .Create();

    private static readonly ITransition<Order, string> _ship =
        GetTransitionBuilder<string>()
            .Requires((o, _) => o.IsPaid, "Order must be paid before shipping.")
            .Requires((_, tracking) => !string.IsNullOrWhiteSpace(tracking), "Tracking number must not be empty.")
            .Do((o, tracking) =>
            {
                o.IsShipped = true;
                o.TrackingNumber = tracking;
            })
            .Create();

    public void Pay() => _pay.Invoke(this);
    public void Cancel() => _cancel.Invoke(this);
    public void Ship(string trackingNumber) => _ship.Invoke(this, trackingNumber);

    public void RaiseViolation(string message) => AddViolationMessage(message);

    public void ForceShip() =>
        GetTransitionBuilder()
            .Do(o => o.IsShipped = true)
            .Create()
            .Invoke(this);
}
