using Microsoft.Extensions.DependencyInjection;

namespace Bllueprint.Core.Domain;

public static class BllueprintServiceCollectionExtensions
{
    public static IServiceCollection AddBllueprintDomainServices(
        this IServiceCollection services)
    {
        services.AddScoped<INotificationContext>(sp =>
        {
            var notificationContext = new NotificationContext();
            DomainNotifications.SetCurrent(notificationContext);
            return notificationContext;
        });

        return services;
    }
}
