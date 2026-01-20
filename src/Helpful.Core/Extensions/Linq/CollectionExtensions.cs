namespace Helpful.Core.Extensions.Linq;

public static class CollectionExtensions
{
    /// <summary>
    /// Удаляет набор элементов из коллекции.
    /// </summary>
    /// <param name="collection"> Коллекция. </param>
    /// <param name="items"> Удаляемые элементы. </param>
    /// <param name="throwException"> Кидать ли исключение если элемента нет. </param>
    public static void RemoveRange<T>(
        this ICollection<T> collection,
        IEnumerable<T> items,
        bool throwException = false)

    {
        foreach (var item in items)
        {
            if (!collection.Remove(item) && throwException)
                throw new Exception($"Не удалось удалить элемент {item}");
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
        var index = list.IndexOf(element);
        if (index != 0)
            return list[index - 1];

        return isCycle ? list[list.Count - 1] : default;
    }

    /// <summary>
    /// Получает по указанному элементу, следующее элемент или null если это последнее элемент.
    /// </summary>
    /// <param name="list"> Список. </param>
    /// <param name="element"> Элемент относительно, которого ищется. </param>
    /// <param name="isCycle"> Зацикленный ли список. </param>
    public static T Next<T>(this IReadOnlyList<T> list, T element, bool isCycle = false)
    {
        var index = list.IndexOf(element);
        if (index != list.Count - 1)
            return list[index + 1];

        return isCycle ? list[0] : throw new HelpfulException("Нет элементов");
    }

    /// <summary>
    /// Возвращает элементы списка в обратном порядке.
    /// </summary>
    public static IEnumerable<T> ReverseList<T>(this IReadOnlyList<T> list)
    {
        for (var index = list.Count - 1; index >= 0; index--)
            yield return list[index];
    }
}