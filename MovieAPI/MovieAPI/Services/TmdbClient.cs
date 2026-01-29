namespace MovieAPI.Services
{
    using MovieAPI.Services.TmdbDtos;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;

    public sealed class TmdbClient(HttpClient http) : ITmdbClient
    {
        public async Task<TmdbPopularResponse> GetPopularMoviesAsync(
            int page,
            CancellationToken cancellationToken = default)
        {
            var response = await http.GetFromJsonAsync<TmdbPopularResponse>(
                $"movie/popular?page={page}",
                cancellationToken);

            return response
                ?? throw new InvalidOperationException("TMDB returned no data.");
        }

        public async Task<TmdbMovieDetail> GetMovieDetailAsync(
            int movieId,
            CancellationToken cancellationToken)
        {
            var response = await http.GetFromJsonAsync<TmdbMovieDetail>(
            $"movie/{movieId}",
            cancellationToken);

            return response
                ?? throw new InvalidOperationException("TMDB returned no data.");
        }
    }
}
