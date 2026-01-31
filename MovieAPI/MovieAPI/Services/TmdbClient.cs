namespace MovieAPI.Services
{
    using MovieAPI.Services.Exceptions;
    using MovieAPI.Services.TmdbDtos;
    using System.Net;
    using System.Net.Http;
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
            using var response = await http.GetAsync($"movie/{movieId}", cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new TmdbNotFoundException($"Movie {movieId} not found.");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<TmdbMovieDetail>(cancellationToken: cancellationToken);

            return result
                ?? throw new InvalidOperationException("TMDB returned no data.");
        }

        public async Task<TmdbPopularResponse> SearchMoviesAsync(
            string query,
            int page,
            CancellationToken cancellationToken = default)
        {
            var encodedQuery = Uri.EscapeDataString(query);

            var response = await http.GetFromJsonAsync<TmdbPopularResponse>(
                $"search/movie?query={encodedQuery}&page={page}",
                cancellationToken);

            return response ?? throw new InvalidOperationException("TMDB returned no data.");
        }

        public async Task<TmdbPaginatedResponse<TmdbBigListMovie>> DiscoverMoviesAsync(
            int page, CancellationToken cancellationToken = default)
        {
            var response = await http.GetFromJsonAsync<TmdbPaginatedResponse<TmdbBigListMovie>>(
                $"discover/movie?sort_by=vote_average.desc&page={page}",
                cancellationToken);

            return response ?? new TmdbPaginatedResponse<TmdbBigListMovie>();
        }
    }
}
