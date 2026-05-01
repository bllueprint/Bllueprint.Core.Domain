using Bllueprint.Core.Domain;
using FluentAssertions;

namespace Bllueprint.Core.Domain.Tests;

public class NotificationScopeTests
{
    [Fact]
    public void NewScope_ShouldStartEmpty()
    {
        using var scope = new NotificationScope();

        scope.NotificationContext.IsEmpty.Should().BeTrue();
    }

    [Fact]
    public void NestedScope_ShouldBeIsolatedFromOuterScope()
    {
        using var outer = new NotificationScope();
        DomainNotifications.Current.Add("outer", "outer error");

        using (var inner = new NotificationScope())
        {
            inner.NotificationContext.IsEmpty.Should().BeTrue();
        }
    }

    [Fact]
    public void Dispose_ShouldRestoreOuterScope()
    {
        using var outer = new NotificationScope();
        DomainNotifications.Current.Add("outer", "outer error");

        using (new NotificationScope()) { /* inner scope */ }

        DomainNotifications.Current.HasErrors.Should().BeTrue();
        DomainNotifications.Current.All[0].Message.Should().Be("outer error");
    }

    [Fact]
    public void NotificationContext_ShouldMatchDomainNotificationsCurrent()
    {
        using var scope = new NotificationScope();
        DomainNotifications.Current.Add("T", "msg");

        scope.NotificationContext.All.Should().ContainSingle()
            .Which.Message.Should().Be("msg");
    }
}
