namespace Helpful.Core.Extensions;

public static class DateTimeExtensions
{
    /// <summary>
    /// The number of milliseconds since
    /// the "Unix epoch" 1970-01-01T00:00:00Z (UTC).
    /// This value is independent of the time zone.
    /// </summary>
    public static DateTime GetLikeUtc(this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute,
            dateTime.Second, dateTime.Millisecond, DateTimeKind.Utc);
    }

    /// <summary>
    /// The DateTime since the "Unix epoch" 1970-01-01T00:00:00Z (UTC).
    /// </summary>
    public static DateTime ToDateTimeSinceEpoch(this long timeStamp)
    {
        return DateTimeOffset.FromUnixTimeMilliseconds(timeStamp).DateTime;
    }

    /// <summary>
    /// The number of milliseconds since
    /// the "Unix epoch" 1970-01-01T00:00:00Z (UTC).
    /// This value is independent of the time zone.
    /// </summary>
    public static long ToMillisecondsSinceEpoch(this DateTime dateTime)
    {
        var dateTimeUtc = dateTime.Kind == DateTimeKind.Unspecified 
            ? dateTime.GetLikeUtc() 
            : dateTime;
        
        return new DateTimeOffset(dateTimeUtc).ToUnixTimeMilliseconds();
    }
}