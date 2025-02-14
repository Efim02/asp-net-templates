namespace Helpful.Api.Parameters;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

/// <summary>
/// Пользовательская реализация ограничителя для работы с Enum-ом.
/// </summary>
/// <typeparam name="TEnum"> Тип Enum-а. </typeparam>
/// <remarks> Это необходимо чтобы Enum в запросах и DTO-хах парсился в string. </remarks>
public class CustomRouteConstraint<TEnum> : IRouteConstraint where TEnum : struct, Enum
{
    /// <inheritdoc />
    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values,
        RouteDirection routeDirection)
    {
        var routeValueString = values[routeKey]?.ToString();
        return Enum.TryParse(routeValueString, true, out TEnum _);
    }
}