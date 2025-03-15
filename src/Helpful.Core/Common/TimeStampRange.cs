namespace Helpful.Core.Common;

/// <summary>
/// Range - для дат. Отметки времени в UTC.
/// </summary>
public sealed class TimeStampRange
{
    /// <summary>
    /// Range - для дат.
    /// </summary>
    /// <param name="start"> Дата начала. </param>
    /// <param name="end"> Дата окончания. </param>
    public TimeStampRange(long start, long end)
    {
        Start = start;
        End = end;
    }

    /// <summary>
    /// Дата начала.
    /// </summary>
    public long Start { get; }

    /// <summary>
    /// Дата окончания.
    /// </summary>
    public long End { get; }

    /// <summary>
    /// Входит отметка времени UTC в Range.
    /// </summary>
    /// <returns> TRUE - если входит, после Start или меньше или равно End. </returns>
    public bool IsEntry(long timeStamp)
    {
        return timeStamp > Start && timeStamp <= End;
    }
}