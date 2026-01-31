namespace MovieAPI.Services
{
    using TmdbDtos;

    public interface ITmdbClient
    {
        Task<TmdbPopularResponse> GetPopularMoviesAsync(
            int page,
            CancellationToken cancellationToken = default);

        Task<TmdbMovieDetail> GetMovieDetailAsync(
            int movieId,
            CancellationToken cancellationToken = default);

        Task<TmdbPopularResponse> SearchMoviesAsync(
            string query,
            int page,
            CancellationToken cancellationToken = default);

        Task<TmdbPaginatedResponse<TmdbBigListMovie>> DiscoverMoviesAsync(
            int page,
            CancellationToken cancellationToken = default
        );

    }
}
