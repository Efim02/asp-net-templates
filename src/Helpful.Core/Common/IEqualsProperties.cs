namespace Helpful.Core.Common;

/// <summary>
/// Может сравниваться с типом T по свойствам.
/// </summary>
public interface IEqualsProperties
{
    /// <summary>
    /// Сравнивает текущий объект с указанным по свойствам.
    /// </summary>
    bool EqualsByProperties(object other);
}

/// <summary>
/// Может сравниваться с типом T по свойствам.
/// </summary>
/// <typeparam name="T"> Тип. </typeparam>
public interface IEqualsProperties<in T>
{
    /// <summary>
    /// Сравнивает текущий объект с указанным по свойствам.
    /// </summary>
    bool EqualsByProperties(T other);
}

/// <summary>
/// Расширение для работы с типом IEqualsProperties.
/// </summary>
public static class EqualsPropertiesExtensions
{
    /// <summary>
    /// Сравнивает по свойствам объекты, которые могут быть null.
    /// </summary>
    public static bool EqualsByPropertiesNullables<T>(
        this IEqualsProperties<T>? obj,
        IEqualsProperties<T>? other)
    {
        return (obj is null && other is null) ||
               (obj is not null && other is not null && obj.EqualsByProperties((T)other));
    }

    /// <summary>
    /// Сравнивает по свойствам объекты, один из которых может быть null.
    /// </summary>
    public static bool EqualsByPropertiesNullable<T>(
        this IEqualsProperties<T> obj,
        IEqualsProperties<T>? other)
    {
        return other is not null && obj.EqualsByProperties((T)other);
    }

    /// <summary>
    /// Сравнивает коллекции.
    /// </summary>
    /// <returns> True - если коллекции по кол-ву и элементы по свойствам равны. </returns>
    public static bool EqualsByProperties<T>(
        this IEnumerable<IEqualsProperties<T>> enumerable1,
        IEnumerable<IEqualsProperties<T>> enumerable2,
        Func<T, object>? getOrderProperty = null)
    {
        var list1 = GetOrderedList(enumerable1, getOrderProperty);
        var list2 = GetOrderedList(enumerable2, getOrderProperty);

        if (list1.Count != list2.Count)
            return false;

        for (var index = 0; index < list1.Count; index++)
        {
            var element1 = list1[index];
            var element2 = list2[index];

            if (element1.EqualsByProperties((T)element2))
                continue;

            return false;
        }

        return true;
    }

    private static List<IEqualsProperties<T>> GetOrderedList<T, TP>(
        IEnumerable<IEqualsProperties<T>> enumerable1,
        Func<T, TP>? getOrderProperty)
    {
        return getOrderProperty == null
            ? enumerable1.ToList()
            : enumerable1.Select(e => (T)e)
                .OrderBy(getOrderProperty)
                .Cast<IEqualsProperties<T>>()
                .ToList();
    }
}