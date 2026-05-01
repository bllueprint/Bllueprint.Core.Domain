namespace Bllueprint.Core.Domain.Tests;

public abstract class DomainTest : IDisposable
{
    private readonly NotificationScope _scope = new();

    ~DomainTest()
    {
        Dispose(false);
    }

    protected INotificationContext Notifications => _scope.NotificationContext;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _scope.Dispose();
        }
    }
}
