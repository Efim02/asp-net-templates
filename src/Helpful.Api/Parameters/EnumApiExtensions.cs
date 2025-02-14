namespace Helpful.Api.Parameters;

using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

public static class EnumApiExtensions
{
    /// <summary>
    /// Регистрирует текстовое представление для укзанного Enum, в запросах.
    /// </summary>
    public static IServiceCollection RegisterEnumString<TEnum>(this IServiceCollection serviceCollection)
        where TEnum : struct, Enum
    {
        serviceCollection.AddControllersWithViews().AddJsonOptions(options =>
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        return serviceCollection.Configure<RouteOptions>(options =>
            options.ConstraintMap.Add(nameof(TEnum), typeof(CustomRouteConstraint<TEnum>)));
    }
}