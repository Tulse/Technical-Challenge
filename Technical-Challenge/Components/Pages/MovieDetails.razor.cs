namespace Technical_Challenge.Components.Pages
{
    using Microsoft.AspNetCore.Components;
    using Services;

    public partial class MovieDetails(IMoviesApiClient moviesApi) : ComponentBase
    {
        [Parameter] public int MovieId { get; set; }

        private bool _isLoading = true;
        private string? _errorMessage;
        private Models.MovieDetail? _movie;

        protected override async Task OnParametersSetAsync()
        {
            try
            {
                _movie = await moviesApi.GetMovieDetailAsync(MovieId);
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
            }
            finally
            {
                _isLoading = false;
            }
        }
    }
}
