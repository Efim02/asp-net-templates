namespace Helpful.Core.Extensions.Linq;

/// <summary>
/// Расширение для Stack.
/// </summary>
public static class StackExtensions
{
    /// <summary>
    /// Достает элемент или если нет элементов возвращает null.
    /// </summary>
    public static T? PopOrDefault<T>(this Stack<T> stack)
    {
        return stack.Count == 0 ? default : stack.Pop();
    }

    /// <summary>
    /// Просматривает элемент или если нет элементов возвращает null.
    /// </summary>
    public static T? PeekOrDefault<T>(this Stack<T> stack)
    {
        return stack.Count == 0 ? default : stack.Peek();
    }
}