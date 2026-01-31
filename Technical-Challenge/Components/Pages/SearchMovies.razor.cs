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
    private string _sortLabel = string.Empty;
    private SortDirection _sortDirection = SortDirection.None;

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
}
