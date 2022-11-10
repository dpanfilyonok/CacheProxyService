namespace CacheProxyService.Models.Exceptions;

public class UnableToLocateException : Exception
{
    public UnableToLocateException() { }
    public UnableToLocateException(string message) : base(message) { }
    public UnableToLocateException(string message, Exception innerException) : base(message, innerException)
    { }

    public UnableToLocateException(GeoCoordinates coords)
        : base($"Unable to locate address on {coords}")
    {
    }
}
