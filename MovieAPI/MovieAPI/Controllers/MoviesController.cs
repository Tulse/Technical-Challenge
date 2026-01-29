namespace MovieAPI.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Services;
    using System.Threading;

    [Route("api/movies")]
    [ApiController]
    public class MoviesController(IMovieService movieService) : ControllerBase
    {
        [HttpGet("popular")]
        public async Task<IActionResult> GetPopular(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken cancellationToken = default)
        {
            var result = await movieService.GetPopularMoviesAsync(
            page,
            pageSize,
            cancellationToken);

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(
            int id,
            CancellationToken cancellationToken = default)
        {   
            var movie = await movieService.GetMovieDetailAsync(id, cancellationToken);
            return Ok(movie);
        }
    }
}
