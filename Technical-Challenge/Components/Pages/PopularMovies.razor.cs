namespace Technical_Challenge.Components.Pages
{
    using Microsoft.AspNetCore.Components;
    using Models;
    using Services;

    public partial class PopularMovies(IMoviesApiClient moviesApi) : ComponentBase
    {
        private bool _isLoading = true;
        private string? _errorMessage;
        private List<MovieSummary> _movies = [];

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var response = await moviesApi.GetPopularMoviesAsync();
                _movies = response.Items.ToList();
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
    }
}
