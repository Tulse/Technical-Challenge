namespace Technical_Challenge.Components.Pages
{
    using System.Net.Http.Json;
    using Microsoft.AspNetCore.Components;

    public partial class PopularMovies(IHttpClientFactory httpClientFactory) : ComponentBase
    {
        private bool _isLoading = true;
        private string? _errorMessage;
        private List<MovieSummary> _movies = [];

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var http = httpClientFactory.CreateClient("MoviesApi");

                var response = await http.GetFromJsonAsync<PagedResponse<MovieSummary>>(
                    "api/movies/popular?page=1&pageSize=20");

                _movies = response?.Items?.ToList() ?? [];
            }
            catch (Exception ex)
            {
                _errorMessage = $"Failed to load movies: {ex.Message}";
            }
            finally
            {
                _isLoading = false;
            }
        }

        public sealed record MovieSummary(
            int Id,
            string Title,
            string? PosterUrl,
            string? ReleaseDate,
            double Rating);

        public sealed record PagedResponse<T>(
            int Page,
            int PageSize,
            int TotalPages,
            int TotalResults,
            IReadOnlyList<T> Items);
    }
}
