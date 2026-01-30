namespace Technical_Challenge.Models
{
    public sealed class PagedResponse<T>
    {
        public int Page { get; init; }
        public int PageSize { get; init; }
        public int TotalPages { get; init; }
        public int TotalResults { get; init; }

        public List<T> Items { get; init; } = [];
    }
}
