namespace Helpful.Api.Extensions;

public static class DateTimeExtensions
{
    /// <summary>
    /// The number of milliseconds since
    /// the "Unix epoch" 1970-01-01T00:00:00Z (UTC).
    /// This value is independent of the time zone.
    /// </summary>
    public static long MillisecondsSinceEpoch(this DateTime dateTime)
    {
        var universalDateTime = dateTime.ToUniversalTime();
        return new DateTimeOffset(universalDateTime).ToUnixTimeMilliseconds();
    }
    
    /// <summary>
    /// The DateTime since the "Unix epoch" 1970-01-01T00:00:00Z (UTC).
    /// </summary>
    public static DateTime DateTimeSinceEpoch(this long timeStamp)
    {
        return DateTimeOffset.FromUnixTimeMilliseconds(timeStamp).UtcDateTime;
    }
}