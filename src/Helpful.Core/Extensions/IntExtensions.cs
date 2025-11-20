namespace SYNC.BL.Extensions;

using System;

/// <summary>
/// Утилита для типа Int.
/// </summary>
public static class IntExtensions
{
    /// <summary>
    /// Сжатая реализация округления до целого числа.
    /// </summary>
    /// <param name="value"> Значение. </param>
    /// <returns> Результат округления. </returns>
    public static int Round(this double value)
    {
        return (int) Math.Round(value);
    }

    /// <summary>
    /// Берет число в промежутке.
    /// </summary>
    public static int Clamp(this int value, int min, int max)
    {
        return value < min ? min : value > max ? max : value;
    }
}
