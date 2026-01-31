namespace MovieAPI.Services.TmdbDtos
{
    public sealed class TmdbPaginatedResponse<T>
    {
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int TotalResults { get; set; }
        public List<T> Results { get; set; } = new();
    }
}
