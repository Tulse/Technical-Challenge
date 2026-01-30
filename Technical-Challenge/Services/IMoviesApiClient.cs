namespace Technical_Challenge.Services
{
    using Models;

    public interface IMoviesApiClient
    {
        Task<PagedResponse<MovieSummary>> GetPopularMoviesAsync(
            int page = 1,
            int pageSize = 20,
            CancellationToken cancellationToken = default);

        Task<MovieDetail> GetMovieDetailAsync(
            int movieId,
            CancellationToken cancellationToken = default);

        Task<PagedResponse<MovieSummary>> SearchMoviesAsync(
            string query,
            int page = 1,
            int pageSize = 20,
            CancellationToken cancellationToken = default);
    }
}
