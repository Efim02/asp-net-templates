namespace Helpful.Api.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Расширение для <see cref="IServiceCollection" />
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Удаляет сервис из коллекции сервисов по указанному типу.
    /// </summary>
    /// <typeparam name="TService"> Тип сервиса. </typeparam>
    /// <param name="services"> Коллекция сервисов. </param>
    public static void RemoveService<TService>(this IServiceCollection services)
    {
        services.Remove(services.First(s => s.ServiceType == typeof(TService)));
    }

    /// <summary>
    /// Заменяет сервис в коллекции сервисов.
    /// </summary>
    /// <typeparam name="TService"> Тип абстракции сервиса. </typeparam>
    /// <param name="services"> Коллекция сервисов. </param>
    /// <param name="implFactory"> Фабрика создания реализации. </param>
    /// <param name="serviceLifetime"> Жизненный цикл сервиса. </param>
    public static void ReplaceService<TService>(
        this IServiceCollection services,
        Func<IServiceProvider, TService> implFactory,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where TService : class
    {
        services.RemoveService<TService>();

        switch (serviceLifetime)
        {
            case ServiceLifetime.Singleton:
                services.AddSingleton(implFactory);
                break;
            case ServiceLifetime.Scoped:
                services.AddScoped(implFactory);
                break;
            case ServiceLifetime.Transient:
                services.AddTransient(implFactory);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(serviceLifetime),
                    serviceLifetime,
                    @"Не реализованы другие типы жизней.");
        }
    }
}