using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Bllueprint.Core.Domain.Tests;

public class BllueprintServiceCollectionExtensionsTests
{
    [Fact]
    public void AddBllueprintDomainServices_ShouldRegisterINotificationContext()
    {
        var services = new ServiceCollection();
        services.AddBllueprintDomainServices();

        ServiceProvider provider = services.BuildServiceProvider();
        using IServiceScope scope = provider.CreateScope();

        INotificationContext? context = scope.ServiceProvider.GetService<INotificationContext>();

        context.Should().NotBeNull();
    }

    [Fact]
    public void AddBllueprintDomainServices_ShouldRegisterAsScoped()
    {
        var services = new ServiceCollection();
        services.AddBllueprintDomainServices();

        ServiceDescriptor? descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(INotificationContext));

        descriptor.Should().NotBeNull();
        descriptor!.Lifetime.Should().Be(ServiceLifetime.Scoped);
    }

    [Fact]
    public void AddBllueprintDomainServices_ShouldSetDomainNotificationsCurrentToRegisteredInstance()
    {
        var services = new ServiceCollection();
        services.AddBllueprintDomainServices();

        ServiceProvider provider = services.BuildServiceProvider();
        using IServiceScope scope = provider.CreateScope();

        INotificationContext context = scope.ServiceProvider.GetRequiredService<INotificationContext>();

        DomainNotifications.Current.Should().BeSameAs(context);
    }

    [Fact]
    public void AddBllueprintDomainServices_ShouldReturnSameServiceCollection_ForChaining()
    {
        var services = new ServiceCollection();

        IServiceCollection result = services.AddBllueprintDomainServices();

        result.Should().BeSameAs(services);
    }
}
