namespace Helpful.Core.Common;

/// <summary>
/// Может быть скопирован.
/// </summary>
public interface ICopying<out T>
{
    /// <summary>
    /// Скопирует себя как новый объект.
    /// </summary>
    T Copy();
}

public static class CopyingExtensions
{
    public static IEnumerable<T> Copy<T>(this IEnumerable<ICopying<T>> copyings)
    {
        foreach (var copying in copyings)
        {
            yield return copying.Copy();
        }
    }
    public static List<T> CopyList<T>(this IEnumerable<ICopying<T>> copyings)
    {
        return copyings.Copy().ToList();
    }
}