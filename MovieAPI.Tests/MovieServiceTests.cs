using MovieAPI.Models;
using MovieAPI.Results;
using MovieAPI.Services;
using MovieAPI.Services.Exceptions;
using MovieAPI.Services.TmdbDtos;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace MovieAPI.Tests
{
    public class MovieServiceTests
    {
        // Success

        [Fact]
        public async Task GetMovieDetailAsync_WhenTmdbReturnsMovie_ReturnsSuccessResultWithMappedMovieDetail()
        {
            // Arrange
            var tmdbClient = Substitute.For<ITmdbClient>();

            tmdbClient.GetMovieDetailAsync(123, Arg.Any<CancellationToken>())
                .Returns(new TmdbMovieDetail
                {
                    Id = 123,
                    Title = "Test Movie",
                    Overview = "Test overview",
                    PosterPath = "/poster.jpg",
                    ReleaseDate = "2024-01-01",
                    Runtime = 110,
                    VoteAverage = 7.5,
                    Genres =
                    [
                        new TmdbGenre { Name = "Action" },
                        new TmdbGenre { Name = "Drama" }
                    ]
                });

            var sut = new MovieService(tmdbClient);

            // Act
            var result = await sut.GetMovieDetailAsync(123, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);

            var ok = Assert.IsType<SuccessResult<MovieDetail>>(result);
            Assert.Equal(123, ok.Value.Id);
            Assert.Equal("Test Movie", ok.Value.Title);
            Assert.Equal(110, ok.Value.RuntimeMinutes);
            Assert.Contains("Action", ok.Value.Genres);
            Assert.NotNull(ok.Value.PosterUrl);
            Assert.Contains("/poster.jpg", ok.Value.PosterUrl);
        }

        // Failures

        [Fact]
        public async Task GetMovieDetailAsync_WhenMovieIdIsInvalid_ReturnsValidationError()
        {
            // Arrange
            var tmdbClient = Substitute.For<ITmdbClient>();
            var sut = new MovieService(tmdbClient);

            // Act
            var result = await sut.GetMovieDetailAsync(movieId: 0, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            var error = Assert.IsType<ValidationErrorResult<MovieDetail>>(result);
            Assert.Contains("MovieId", error.Message);
        }

        [Fact]
        public async Task GetMovieDetailAsync_WhenTmdbReturnsNotFound_ReturnsNotFoundResult()
        {
            // Arrange
            var tmdbClient = Substitute.For<ITmdbClient>();

            tmdbClient
                .GetMovieDetailAsync(999, Arg.Any<CancellationToken>())
                .Throws(new TmdbNotFoundException("Movie not found"));

            var sut = new MovieService(tmdbClient);

            // Act
            var result = await sut.GetMovieDetailAsync(999, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            var notFound = Assert.IsType<NotFoundResult<MovieDetail>>(result);
            Assert.Contains("not found", notFound.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task GetMovieDetailAsync_WhenTmdbThrowsHttpRequestException_ReturnsExternalServiceError()
        {
            // Arrange
            var tmdbClient = Substitute.For<ITmdbClient>();

            tmdbClient
                .GetMovieDetailAsync(123, Arg.Any<CancellationToken>())
                .Throws(new HttpRequestException("TMDB unavailable"));

            var sut = new MovieService(tmdbClient);

            // Act
            var result = await sut.GetMovieDetailAsync(123, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            var error = Assert.IsType<ExternalServiceErrorResult<MovieDetail>>(result);
            Assert.Contains("TMDB", error.Message);
        }

        [Fact]
        public async Task GetPopularMoviesAsync_WhenPageSizeIsSmallerThanResults_TrimsResultsCorrectly()
        {
            // Arrange
            var tmdbClient = Substitute.For<ITmdbClient>();

            tmdbClient
                .GetPopularMoviesAsync(1, Arg.Any<CancellationToken>())
                .Returns(new TmdbPopularResponse
                {
                    Page = 1,
                    TotalPages = 1,
                    TotalResults = 3,
                    Results =
                    {
                        new TmdbMovie { Id = 1, Title = "Movie 1" },
                        new TmdbMovie { Id = 2, Title = "Movie 2" },
                        new TmdbMovie { Id = 3, Title = "Movie 3" },
                        new TmdbMovie { Id = 4, Title = "Movie 4" },
                        new TmdbMovie { Id = 5, Title = "Movie 5" }
                    }
                });

            var sut = new MovieService(tmdbClient);

            // Act
            var result = await sut.GetPopularMoviesAsync(page: 1, pageSize: 2, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);

            var ok = Assert.IsType<SuccessResult<PagedResponse<MovieSummary>>>(result);
            Assert.Equal(2, ok.Value.Items.Count);
            Assert.Equal("Movie 1", ok.Value.Items[0].Title);
            Assert.Equal("Movie 2", ok.Value.Items[1].Title);
        }
    }
}
