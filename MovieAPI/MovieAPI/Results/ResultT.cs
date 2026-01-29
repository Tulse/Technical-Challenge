namespace MovieAPI.Results
{
    public abstract record Result<T> : Result
    {
        protected Result(bool isSuccess) : base(isSuccess) { }
    }
}
