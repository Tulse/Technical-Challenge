namespace MovieAPI.Models
{
    public sealed record MovieDetail(
    int Id,
    string Title,
    string Overview,
    string? PosterUrl,
    string? ReleaseDate,
    int? RuntimeMinutes,
    double Rating,
    IReadOnlyList<string> Genres);
}
