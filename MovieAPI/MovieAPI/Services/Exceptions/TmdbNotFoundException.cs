namespace MovieAPI.Services.Exceptions
{
    public sealed class TmdbNotFoundException(string message) : Exception(message);
}
