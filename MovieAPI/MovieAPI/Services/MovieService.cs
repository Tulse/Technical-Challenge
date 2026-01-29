namespace MovieAPI.Services
{
    using Models;
    using TmdbDtos;

    public sealed class MovieService(ITmdbClient tmdbClient) : IMovieService
    {
        private const string PosterBaseUrl = "https://image.tmdb.org/t/p/w342";

        public async Task<PagedResponse<MovieSummary>> GetPopularMoviesAsync(
            int page,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var tmdb = await tmdbClient.GetPopularMoviesAsync(page, cancellationToken);

            var items = tmdb.Results
                .Take(pageSize)
                .Select(MapMovie)
                .ToList();

            return new PagedResponse<MovieSummary>(
                tmdb.Page,
                pageSize,
                tmdb.TotalPages,
                tmdb.TotalResults,
                items);
        }

        public async Task<MovieDetail> GetMovieDetailAsync(
            int movieId, 
            CancellationToken cancellationToken)
        {
            var tmdb = await tmdbClient.GetMovieDetailAsync(movieId, cancellationToken);

            return new MovieDetail(
            tmdb.Id,
            tmdb.Title,
            tmdb.Overview,
            tmdb.PosterPath is null ? null : $"{PosterBaseUrl}{tmdb.PosterPath}",
            tmdb.ReleaseDate,
            tmdb.Runtime,
            tmdb.VoteAverage,
            tmdb.Genres.Select(g => g.Name).ToList()
            );
        }

        private static MovieSummary MapMovie(TmdbMovie movie) =>
            new(
                movie.Id,
                movie.Title,
                movie.PosterPath is null ? null : $"{PosterBaseUrl}{movie.PosterPath}",
                movie.ReleaseDate,
                movie.VoteAverage
            );
    }
}
