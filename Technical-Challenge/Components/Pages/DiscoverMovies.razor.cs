namespace Technical_Challenge.Components.Pages
{
    using Microsoft.AspNetCore.Components;
    using Models;
    using Services;

    public partial class DiscoverMovies(IMoviesApiClient moviesApi) : ComponentBase
        {
            protected List<MovieDiscover> _movies = new();
            protected bool _isLoading;
            protected string? _errorMessage;

            protected override async Task OnInitializedAsync()
            {
                _isLoading = true;

                try
                {
                    _movies = await moviesApi.DiscoverMoviesAsync();
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
