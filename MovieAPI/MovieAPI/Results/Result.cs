namespace MovieAPI.Results
{
    public abstract record Result
    {
        public bool IsSuccess { get; }

        protected Result(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }
    }
}
