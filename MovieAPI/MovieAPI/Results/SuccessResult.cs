namespace MovieAPI.Results
{
    public sealed record SuccessResult<T>(T Value) : Result<T>(true);
}
