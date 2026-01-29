namespace MovieAPI.Services
{
    using Models;

    public interface IMovieService
    {
        Task<PagedResponse<MovieSummary>> GetPopularMoviesAsync(
            int page,
            int pageSize,
            CancellationToken cancellationToken = default);

        Task<MovieDetail> GetMovieDetailAsync(
            int movieId,
            CancellationToken cancellationToken = default);
    }
}
