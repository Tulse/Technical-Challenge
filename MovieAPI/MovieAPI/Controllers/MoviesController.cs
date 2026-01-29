using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MovieAPI.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        [HttpGet("popular")]
        public IActionResult GetPopularMovies(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
        {
            var movies = new[]
            {
            new
            {
                Id = 1,
                Title = "Scary Movie",
                PosterUrl = null as string,
                ReleaseDate = "2024-01-01",
                Rating = 7.8
            },
            new
            {
                Id = 2,
                Title = "Not Another Teen Movie",
                PosterUrl = null as string,
                ReleaseDate = "2023-10-15",
                Rating = 6.9
            }
        };

            return Ok(new
            {
                Page = page,
                PageSize = pageSize,
                TotalPages = 1,
                TotalResults = movies.Length,
                Items = movies
            });
        }
    }
}
