namespace Technical_Challenge.Models
{
    public sealed record MovieDiscover(
        int Id,
        string Title,
        string Overview,
        string? PosterUrl,
        string? ReleaseDate,
        double Rating
    );
}
