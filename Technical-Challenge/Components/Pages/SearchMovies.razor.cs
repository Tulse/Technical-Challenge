using Microsoft.AspNetCore.Components;
using MudBlazor;
using Technical_Challenge.Models;
using Technical_Challenge.Services;

namespace Technical_Challenge.Components.Pages;

public partial class SearchMovies : ComponentBase
{
    private string _query = string.Empty;

    private bool _isLoading;
    private bool _hasSearched;
    private string? _errorMessage;
    private double _minRating = 0;
    private int? _releaseYear;

    private List<MovieSummary> _movies = [];

    [Inject]
    public IMoviesApiClient MoviesApi { get; set; } = default!;

    private async Task SearchAsync()
    {
        _errorMessage = null;
        _hasSearched = true;

        if (string.IsNullOrWhiteSpace(_query))
        {
            _movies.Clear();
            _errorMessage = "Please enter a search term.";
            return;
        }

        _isLoading = true;

        try
        {
            var response = await MoviesApi.SearchMoviesAsync(_query, page: 1, pageSize: 20);
            _movies = response.Items.ToList();
        }
        catch (Exception ex)
        {
            _movies.Clear();
            _errorMessage = $"Search failed: {ex.Message}";
        }
        finally
        {
            _isLoading = false;
        }
    }

    private void ResetFilters()
    {
        _minRating = 0;
        _releaseYear = null;
    }

    private bool HasActiveFilters =>
    _minRating > 0 || _releaseYear.HasValue;

    private IEnumerable<MovieSummary> FilteredMovies =>
    _movies
        .Where(m => m.Rating >= _minRating)
        .Where(m =>
            !_releaseYear.HasValue ||
            (m.ReleaseDate?.Length >= 4 &&
             int.TryParse(m.ReleaseDate[..4], out var year) &&
             year == _releaseYear));
}
