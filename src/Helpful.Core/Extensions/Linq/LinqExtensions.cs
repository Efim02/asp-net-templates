namespace Helpful.Core.Extensions.Linq;

using Helpful.Core.Extensions.Collections;

public static class LinqExtensions
{
    /// <summary>
    /// Пустое ли перечисление.
    /// </summary>
    public static bool Empty<T>(this IEnumerable<T> enumerable)
    {
        return !enumerable.Any();
    }

    public static bool EqualsEnumerable<T>(this IEnumerable<T> enumerable, ICollection<T> other)
    {
        foreach (var item in enumerable)
        {
            if (other.All(o => o!.Equals(item)))
                return false;
        }

        return true;
    }

    /// <summary>
    /// Имеются ли элементы в перечисление.
    /// </summary>
    public static bool Has<T>(this IEnumerable<T> enumerable)
    {
        return enumerable.Any();
    }

    public static IEnumerable<T> WhereIf<T>(
        this IEnumerable<T> query,
        Func<T, bool> whereQueryFunc,
        bool condition)
    {
        return condition ? query.Where(whereQueryFunc) : query;
    }

    /// <summary>
    /// Получает индекс элемента в последовательности. -1 - значит не содержится.
    /// </summary>
    public static int IndexOf<T>(this IEnumerable<T> source, T element)
    {
        var index = 0;
        foreach (var item in source)
        {
            if (item!.Equals(element))
                return index;
            index++;
        }

        return -1;
    }

    /// <summary>
    /// Получает номер элемента в последовательности. 0 - значит не содержится.
    /// </summary>
    public static int NumberOf<T>(this IEnumerable<T> source, T element)
    {
        return source.IndexOf(element) + 1;
    }
    
    /// <summary>
    /// Получает по указанному элементу, следующее элемент или null если это последнее элемент.
    /// </summary>
    /// <param name="enumerable"> Список. </param>
    /// <param name="element"> Элемент относительно, которого ищется. </param>
    /// <param name="isCycle"> Зацикленный ли список. </param>
    public static T? Next<T>(this IEnumerable<T> enumerable, T element, bool isCycle = false)
    {
        return CollectionExtensions.Next(enumerable.ToList(), element, isCycle);
    }
}