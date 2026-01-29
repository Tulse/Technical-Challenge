namespace MovieAPI.Services.TmdbDtos
{
    using System.Text.Json.Serialization;

    public class TmdbPopularResponse
    {
        [JsonPropertyName("page")]
        public int Page { get; init; }

        [JsonPropertyName("total_pages")]
        public int TotalPages { get; init; }

        [JsonPropertyName("total_results")]
        public int TotalResults { get; init; }

        [JsonPropertyName("results")]
        public List<TmdbMovie> Results { get; init; } = [];
    }

    public sealed class TmdbMovie
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("title")]
        public string Title { get; init; } = string.Empty;

        [JsonPropertyName("poster_path")]
        public string? PosterPath { get; init; }

        [JsonPropertyName("release_date")]
        public string? ReleaseDate { get; init; }

        [JsonPropertyName("vote_average")]
        public double VoteAverage { get; init; }
    }
}
