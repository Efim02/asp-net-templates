namespace Helpful.Core.Extensions.Linq;

/// <summary>
/// Comparer для сравнения по свойству.
/// </summary>
public class ByPropertyComparer<T, TP> : IEqualityComparer<T>
{
    /// <summary>
    /// Функция получения свойства для сравнения объектов по свойству.
    /// </summary>
    private readonly Func<T, TP> _getPropertyFunc;

    /// <summary>
    /// Comparer для сравнения по свойству.
    /// </summary>
    public ByPropertyComparer(Func<T, TP> getPropertyFunc)
    {
        _getPropertyFunc = getPropertyFunc;
    }

    /// <inheritdoc />
    public bool Equals(T? x, T? y)
    {
        return _getPropertyFunc(x!)!.Equals(_getPropertyFunc(y!));
    }

    /// <inheritdoc />
    public int GetHashCode(T obj)
    {
        return _getPropertyFunc(obj)!.GetHashCode();
    }
}