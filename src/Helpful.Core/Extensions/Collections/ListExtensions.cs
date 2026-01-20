namespace Helpful.Core.Extensions.Collections;

using Helpful.Core.Extensions.Linq;

/// <summary>
/// Расширения для CollectionT.
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    /// Пустой ли список.
    /// </summary>
    public static bool Empty<T>(this ICollection<T> list)
    {
        return list.Count < 1;
    }

    /// <summary>
    /// Имеются ли элементы в списке.
    /// </summary>
    public static bool Has<T>(this ICollection<T> list)
    {
        return list.Count > 0;
    }

    /// <summary>
    /// Удаляет набор элементов из списка.
    /// </summary>
    public static void RemoveRange<T>(this ICollection<T> list, IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            list.Remove(item);
        }
    }

    /// <summary>
    /// Получает по указанному элементу, предыдущий элемент или null если это первый элемент.
    /// </summary>
    /// <param name="list"> Список. </param>
    /// <param name="element"> Элемент относительно, которого ищется. </param>
    /// <param name="isCycle"> Зацикленный ли список. </param>
    public static T? Previous<T>(this IReadOnlyList<T> list, T element, bool isCycle = false)
    {
        if (element is null)
            throw new ArgumentNullException(nameof(element));

        var index = list.IndexOf(element);
        if (index != 0)
            return list[index - 1];

        return isCycle ? list[^1] : default;
    }

    /// <summary>
    /// Получает по указанному элементу, следующее элемент или null если это последнее элемент.
    /// </summary>
    /// <param name="list"> Список. </param>
    /// <param name="element"> Элемент относительно, которого ищется. </param>
    /// <param name="isCycle"> Зацикленный ли список. </param>
    public static T? Next<T>(this IReadOnlyList<T> list, T element, bool isCycle = false)
    {
        if (element is null)
            throw new ArgumentNullException(nameof(element));

        var index = list.IndexOf(element);
        if (index != list.Count - 1)
            return list[index + 1];

        return isCycle ? list[0] : default;
    }
}