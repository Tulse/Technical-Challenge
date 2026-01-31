namespace MovieAPI.Services
{
    using Exceptions;
    using Models;
    using Results;
    using TmdbDtos;

    public sealed class MovieService(ITmdbClient tmdbClient) : IMovieService
    {
        private const string PosterBaseUrl = "https://image.tmdb.org/t/p/w342";

        public async Task<Result<PagedResponse<MovieSummary>>> GetPopularMoviesAsync(
            int page,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            if (page < 1)
                return new ValidationErrorResult<PagedResponse<MovieSummary>>("Page must be at least 1.");

            if (pageSize is < 1 or > 50)
                return new ValidationErrorResult<PagedResponse<MovieSummary>>("PageSize must be between 1 and 50.");

            try
            {
                var tmdb = await tmdbClient.GetPopularMoviesAsync(page, cancellationToken);

                var items = tmdb.Results
                    .Take(pageSize)
                    .Select(MapMovie)
                    .ToList();

                return new SuccessResult<PagedResponse<MovieSummary>>(
                    new PagedResponse<MovieSummary>(
                        tmdb.Page,
                        pageSize,
                        tmdb.TotalPages,
                        tmdb.TotalResults,
                        items)
                    );
            }
            catch (HttpRequestException ex)
            {
                return new ExternalServiceErrorResult<PagedResponse<MovieSummary>>(
                    $"TMDB request failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                return new ExternalServiceErrorResult<PagedResponse<MovieSummary>>(
                    $"Unexpected error while retrieving popular movies: {ex.Message}");
            }
        }

        public async Task<Result<MovieDetail>> GetMovieDetailAsync(
            int movieId, 
            CancellationToken cancellationToken = default)
        {
            if (movieId < 1)
                return new ValidationErrorResult<MovieDetail>("MovieId must be greater than zero.");

            try
            {
                var tmdb = await tmdbClient.GetMovieDetailAsync(movieId, cancellationToken);

                return new SuccessResult<MovieDetail>(
                    new MovieDetail(
                        tmdb.Id,
                        tmdb.Title,
                        tmdb.Overview,
                        tmdb.PosterPath is null ? null : $"{PosterBaseUrl}{tmdb.PosterPath}",
                        tmdb.ReleaseDate,
                        tmdb.Runtime,
                        tmdb.VoteAverage,
                        tmdb.Genres.Select(g => g.Name).ToList()
                    )
                );
            }
            catch (TmdbNotFoundException ex)
            {
                return new NotFoundResult<MovieDetail>(ex.Message);
            }
            catch (HttpRequestException ex)
            {
                return new ExternalServiceErrorResult<MovieDetail>(
                    $"TMDB request failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                return new ExternalServiceErrorResult<MovieDetail>(
                    $"Unexpected error while retrieving movie details: {ex.Message}");
            }
        }

        public async Task<Result<PagedResponse<MovieSummary>>> SearchMoviesAsync(
            string query,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new ValidationErrorResult<PagedResponse<MovieSummary>>("Query must be provided.");

            if (page < 1)
                return new ValidationErrorResult<PagedResponse<MovieSummary>>("Page must be at least 1.");

            if (pageSize is < 1 or > 50)
                return new ValidationErrorResult<PagedResponse<MovieSummary>>("PageSize must be between 1 and 50.");

            try
            {
                var tmdb = await tmdbClient.SearchMoviesAsync(query, page, cancellationToken);

                var items = tmdb.Results
                    .Take(pageSize)
                    .Select(MapMovie)
                    .ToList();

                return new SuccessResult<PagedResponse<MovieSummary>>(
                    new PagedResponse<MovieSummary>(
                        tmdb.Page,
                        pageSize,
                        tmdb.TotalPages,
                        tmdb.TotalResults,
                        items));
            }
            catch (HttpRequestException ex)
            {
                return new ExternalServiceErrorResult<PagedResponse<MovieSummary>>(
                    $"TMDB request failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                return new ExternalServiceErrorResult<PagedResponse<MovieSummary>>(
                    $"Unexpected error while searching movies: {ex.Message}");
            }
        }

        public async Task<Result<List<MovieSummary>>> GetVirtualizeMoviesAsync(
            int pagesToFetch = 10,
            CancellationToken cancellationToken = default)
        {
            var allMovies = new List<MovieSummary>();

            for (var page = 1; page <= pagesToFetch; page++)
            {
                var tmdb = await tmdbClient.DiscoverMoviesAsync(page, cancellationToken);

                var validMovies = tmdb.Results
                    .Where(m => !string.IsNullOrEmpty(m.PosterPath))
                    .Select(m => new MovieSummary(
                        m.Id,
                        m.Title,
                        $"{PosterBaseUrl}{m.PosterPath}",
                        m.ReleaseDate,
                        m.VoteAverage
                    ));

                allMovies.AddRange(validMovies);
            }

            return new SuccessResult<List<MovieSummary>>(allMovies);
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
