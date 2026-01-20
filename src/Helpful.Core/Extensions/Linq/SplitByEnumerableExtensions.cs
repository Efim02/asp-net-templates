namespace Helpful.Core.Extensions.Linq;

/// <summary>
/// Расширение для получения результатов фильтрации: обоих случаев.
/// </summary>
public static class SplitByEnumerableExtensions
{
    /// <summary>
    /// Удаляет набор элементов из колллекции.
    /// </summary>
    public static void RemoveRange<T>(this ICollection<T> collection, IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            collection.Remove(item);
        }
    }

    /// <summary>
    /// Делит коллекцию элементов по предикату, на две коллекции.
    /// </summary>
    /// <returns> True элементы - выполнившие предикат, остальные False - элементы. </returns>
    public static SplitByResult<T> SplitBy<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
    {
        var trueItems = new List<T>();
        var falseItems = new List<T>();
        foreach (var item in enumerable)
        {
            if (predicate.Invoke(item))
                trueItems.Add(item);
            else
                falseItems.Add(item);
        }

        return new SplitByResult<T>
        {
            IsFalse = falseItems,
            IsTrue = trueItems
        };
    }
}

/// <summary>
/// Результат разделения коллекции.
/// </summary>
/// <typeparam name="T"> Тип элемента коллекции. </typeparam>
public class SplitByResult<T>
{
    /// <summary>
    /// Предикат отрицательный.
    /// </summary>
    public IReadOnlyCollection<T> IsFalse { get; set; } = [];

    /// <summary>
    /// Предикат положительный.
    /// </summary>
    public IReadOnlyCollection<T> IsTrue { get; set; } = [];
}