namespace MovieAPI.Services
{
    using Services.TmdbDtos;

    public interface ITmdbClient
    {
        Task<TmdbPopularResponse> GetPopularMoviesAsync(
            int page,
            CancellationToken cancellationToken = default);

        Task<TmdbMovieDetail> GetMovieDetailAsync(
        int movieId,
        CancellationToken cancellationToken = default);
    }
}
