namespace Technical_Challenge.Models
{
    public sealed record PagedResponse<T>(
    int Page,
    int PageSize,
    int TotalPages,
    int TotalResults,
    IReadOnlyList<T> Items);
}
