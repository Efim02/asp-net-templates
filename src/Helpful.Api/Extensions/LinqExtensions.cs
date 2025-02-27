namespace Helpful.Api.Extensions;

public static class LinqExtensions
{
    public static SplitByResult<T> SplitBy<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
    {
        var list = enumerable.ToList();
        return new SplitByResult<T>
        {
            IsTrue = list.Where(predicate).ToList(),
            IsFalse = list.Where(a => !predicate(a)).ToList()
        };
    }
}

/// <summary>
/// Результат разделения коллекции.
/// </summary>
/// <typeparam name="T"> Тип элемента коллекции. </typeparam>
public record SplitByResult<T>
{
    /// <summary>
    /// Предикат положительный.
    /// </summary>
    public required IReadOnlyCollection<T> IsTrue { get; init; }

    /// <summary>
    /// Предикат отрицательный.
    /// </summary>
    public required IReadOnlyCollection<T> IsFalse { get; init; }
}