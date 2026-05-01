using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Bllueprint.Core.Domain.Tests;

public class BlueprintServiceCollectionExtensionsTests
{
    [Fact]
    public void AddBlueprintDomainServices_ShouldRegisterINotificationContext()
    {
        var services = new ServiceCollection();
        services.AddBlueprintDomainServices();

        ServiceProvider provider = services.BuildServiceProvider();
        using IServiceScope scope = provider.CreateScope();

        INotificationContext? context = scope.ServiceProvider.GetService<INotificationContext>();

        context.Should().NotBeNull();
    }

    [Fact]
    public void AddBlueprintDomainServices_ShouldRegisterAsScoped()
    {
        var services = new ServiceCollection();
        services.AddBlueprintDomainServices();

        ServiceDescriptor? descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(INotificationContext));

        descriptor.Should().NotBeNull();
        descriptor!.Lifetime.Should().Be(ServiceLifetime.Scoped);
    }

    [Fact]
    public void AddBlueprintDomainServices_ShouldSetDomainNotificationsCurrentToRegisteredInstance()
    {
        var services = new ServiceCollection();
        services.AddBlueprintDomainServices();

        ServiceProvider provider = services.BuildServiceProvider();
        using IServiceScope scope = provider.CreateScope();

        INotificationContext context = scope.ServiceProvider.GetRequiredService<INotificationContext>();

        DomainNotifications.Current.Should().BeSameAs(context);
    }

    [Fact]
    public void AddBlueprintDomainServices_ShouldReturnSameServiceCollection_ForChaining()
    {
        var services = new ServiceCollection();

        IServiceCollection result = services.AddBlueprintDomainServices();

        result.Should().BeSameAs(services);
    }
}
