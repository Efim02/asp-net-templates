namespace Helpful.Api.Parameters;

using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

public static class EnumApiExtensions
{
    /// <summary>
    /// Регистрирует текстовое представление для укзанного Enum, в запросах.
    /// </summary>
    public static IServiceCollection RegisterEnumStrings<TEnum>(
        this IServiceCollection serviceCollection,
        Dictionary<TEnum, IRouteConstraint> enumConstraints) where TEnum : struct, Enum
    {
        return serviceCollection.Configure<RouteOptions>(options =>
        {
            foreach (var enumConstraintPair in enumConstraints)
                options.ConstraintMap.Add(
                    enumConstraintPair.Key.GetType().Name,
                    enumConstraintPair.Value.GetType());
        });
    }
}