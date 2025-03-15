namespace Sport.Dto.Common;

/// <summary>
/// Range - для дат.
/// </summary>
public struct DateTimeRange
{
    /// <summary>
    /// Range - для дат.
    /// </summary>
    /// <param name="start"> Дата начала. </param>
    /// <param name="end"> Дата окончания. </param>
    public DateTimeRange(DateTime start, DateTime end)
    {
        Start = start;
        End = end;
    }

    /// <summary>
    /// Дата начала.
    /// </summary>
    public DateTime Start { get; }

    /// <summary>
    /// Дата окончания.
    /// </summary>
    public DateTime End { get; }
}