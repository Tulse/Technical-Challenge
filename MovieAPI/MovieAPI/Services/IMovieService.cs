namespace MovieAPI.Services
{
    using Models;
    using Results;

    public interface IMovieService
    {
        Task<Result<PagedResponse<MovieSummary>>> GetPopularMoviesAsync(
            int page,
            int pageSize,
            CancellationToken cancellationToken = default);

        Task<Result<MovieDetail>> GetMovieDetailAsync(
            int movieId,
            CancellationToken cancellationToken = default);

        Task<Result<PagedResponse<MovieSummary>>> SearchMoviesAsync(
            string query,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default);

        Task<Result<List<MovieDiscover>>> GetVirtualizeMoviesAsync(
            int pagesToFetch = 10,
            CancellationToken cancellationToken = default);
    }
}
