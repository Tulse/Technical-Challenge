namespace MovieAPI.Models
{
    public sealed record MovieSummary(
    int Id,
    string Title,
    string? PosterUrl,
    string? ReleaseDate,
    double Rating);
}
