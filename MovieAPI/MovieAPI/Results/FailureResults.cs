namespace MovieAPI.Results
{
    public abstract record FailureResult<T>(string Message) : Result<T>(false);

    public sealed record ValidationErrorResult<T>(string Message) : FailureResult<T>(Message);

    public sealed record NotFoundResult<T>(string Message) : FailureResult<T>(Message);

    public sealed record ExternalServiceErrorResult<T>(string Message) : FailureResult<T>(Message);
}
