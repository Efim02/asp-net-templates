namespace SYNC.BL.Extensions;

using System;

/// <summary>
/// Утилита для типа Int.
/// </summary>
public static class IntUtility
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
    public static int Clamp(this int value, int min = 0, int max = 1)
    {
        return value < min ? min : value > max ? max : value;
    }
}