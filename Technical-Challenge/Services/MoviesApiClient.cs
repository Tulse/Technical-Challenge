namespace Technical_Challenge.Services
{
    using Models;

    public class MoviesApiClient(IHttpClientFactory httpClientFactory) : IMoviesApiClient
    {
        public async Task<PagedResponse<MovieSummary>> GetPopularMoviesAsync(
            int page = 1,
            int pageSize = 20,
            CancellationToken cancellationToken = default)
        {
            var http = httpClientFactory.CreateClient("MoviesApi");

            var response = await http.GetFromJsonAsync<PagedResponse<MovieSummary>>(
                $"movies/popular?page={page}&pageSize={pageSize}",
                cancellationToken);

            return response
                ?? throw new InvalidOperationException("API returned no data.");
        }

        public async Task<MovieDetail> GetMovieDetailAsync(
            int movieId,
            CancellationToken cancellationToken = default)
        {
            var http = httpClientFactory.CreateClient("MoviesApi");

            var response = await http.GetFromJsonAsync<MovieDetail>(
                $"movies/{movieId}",
                cancellationToken);

            return response
                ?? throw new InvalidOperationException("API returned no data.");
        }

        public async Task<PagedResponse<MovieSummary>> SearchMoviesAsync(
            string query,
            int page = 1,
            int pageSize = 20,
            CancellationToken cancellationToken = default)
        {
            var http = httpClientFactory.CreateClient("MoviesApi");

            var encodedQuery = Uri.EscapeDataString(query);

            var response = await http.GetFromJsonAsync<PagedResponse<MovieSummary>>(
                $"movies/search?query={encodedQuery}&page={page}&pageSize={pageSize}",
                cancellationToken);

            return response
                ?? throw new InvalidOperationException("API returned no data.");
        }
    }
}
