namespace MovieAPI.Services.TmdbDtos
{
    using System.Text.Json.Serialization;

    public class TmdbMovieDetail
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("title")]
        public string Title { get; init; } = string.Empty;

        [JsonPropertyName("overview")]
        public string Overview { get; init; } = string.Empty;

        [JsonPropertyName("poster_path")]
        public string? PosterPath { get; init; }

        [JsonPropertyName("release_date")]
        public string? ReleaseDate { get; init; }

        [JsonPropertyName("runtime")]
        public int? Runtime { get; init; }

        [JsonPropertyName("vote_average")]
        public double VoteAverage { get; init; }

        [JsonPropertyName("genres")]
        public List<TmdbGenre> Genres { get; init; } = [];
    }

    public sealed class TmdbGenre
    {
        [JsonPropertyName("name")]
        public string Name { get; init; } = string.Empty;
    }
}
