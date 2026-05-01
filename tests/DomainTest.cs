namespace Bllueprint.Core.Domain.Tests;

public abstract class DomainTest : IDisposable
{
    private readonly NotificationScope _scope = new();

    protected INotificationContext Notifications => _scope.NotificationContext;

    public void Dispose() => _scope.Dispose();
}
