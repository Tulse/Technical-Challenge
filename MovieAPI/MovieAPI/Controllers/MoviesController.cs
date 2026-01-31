namespace MovieAPI.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using Microsoft.AspNetCore.Mvc;
    using MovieAPI.Models;
    using MovieAPI.Results;
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
            var result = await movieService.GetPopularMoviesAsync(page, pageSize, cancellationToken);

            return result switch
            {
                SuccessResult<PagedResponse<MovieSummary>> ok => Ok(ok.Value),
                ValidationErrorResult<PagedResponse<MovieSummary>> ve => BadRequest(ve.Message),
                ExternalServiceErrorResult<PagedResponse<MovieSummary>> ese => StatusCode(502, ese.Message),
                _ => StatusCode(500)
            };
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(
            int id,
            CancellationToken cancellationToken = default)
        {   
            var movie = await movieService.GetMovieDetailAsync(id, cancellationToken);

            return movie switch
            {
                SuccessResult<MovieDetail> ok => Ok(ok.Value),
                NotFoundResult<MovieDetail> nf => NotFound(nf.Message),
                ValidationErrorResult<MovieDetail> ve => BadRequest(ve.Message),
                ExternalServiceErrorResult<MovieDetail> ese => StatusCode(502, ese.Message),
                _ => StatusCode(500)
            };
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string query,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken cancellationToken = default)
        {
            var result = await movieService.SearchMoviesAsync(query, page, pageSize, cancellationToken);

            return result switch
            {
                SuccessResult<PagedResponse<MovieSummary>> ok => Ok(ok.Value),
                ValidationErrorResult<PagedResponse<MovieSummary>> ve => BadRequest(ve.Message),
                ExternalServiceErrorResult<PagedResponse<MovieSummary>> ese => StatusCode(502, ese.Message),
                _ => StatusCode(500)
            };
        }

        [HttpGet("virtualize")]
        public async Task<IActionResult> GetVirtualizeMovies(CancellationToken cancellationToken = default)
        {
            var result = await movieService.GetVirtualizeMoviesAsync(10, cancellationToken);

            return result switch
            {
                SuccessResult<List<MovieSummary>> ok => Ok(ok.Value),
                ValidationErrorResult<List<MovieSummary>> ve => BadRequest(ve.Message),
                ExternalServiceErrorResult<List<MovieSummary>> ese => StatusCode(502, ese.Message),
                _ => StatusCode(500)
            };
        }
    }
}
