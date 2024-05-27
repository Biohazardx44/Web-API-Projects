using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieApp.CustomExceptions;
using MovieApp.Domain.Enums;
using MovieApp.DTOs.MovieDTOs;
using MovieApp.Services.Abstraction;
using System.Security.Claims;

namespace MovieApp.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet] //https://localhost:7118/api/Movies
        public IActionResult GetAllMovies()
        {
            try
            {
                var userId = User.FindFirstValue("userId");

                return Ok(_movieService.GetAllMovies(int.Parse(userId)));
            }
            catch (MovieDataException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Yikes, that's not good! :(");
            }
        }

        [HttpGet("{id}")] //https://localhost:7118/api/Movies/3
        public IActionResult GetMovieByIdRoute([FromRoute] int id)
        {
            try
            {
                return Ok(_movieService.GetById(id));
            }
            catch (MovieDataException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Yikes, that's not good! :(");
            }
        }

        [HttpGet("findById")] //https://localhost:7118/api/Movies/findById?id=2
        public IActionResult GetMovieByIdQuery([FromQuery] int id)
        {
            try
            {
                return Ok(_movieService.GetById(id));
            }
            catch (MovieDataException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Yikes, that's not good! :(");
            }
        }

        [HttpGet("filter")] //https://localhost:7118/api/Movies/filter?genre=2&year=2021
        public IActionResult FilterMovies([FromQuery] Genre? genre, [FromQuery] int? year)
        {
            try
            {
                var userId = User.FindFirstValue("userId");

                return Ok(_movieService.FilterMovies(genre, year, int.Parse(userId)));
            }
            catch (MovieDataException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Yikes, that's not good! :(");
            }
        }

        [HttpPost] //https://localhost:7118/api/Movies
        public IActionResult AddMovie([FromBody] MovieAddDto movieAddDto)
        {
            try
            {
                _movieService.AddMovie(movieAddDto);
                return StatusCode(StatusCodes.Status201Created, $"Movie created!");
            }
            catch (MovieDataException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Yikes, that's not good! :(");
            }
        }

        [HttpDelete] //https://localhost:7118/api/Movies
        public IActionResult DeleteMovieBody([FromBody] int id)
        {
            try
            {
                _movieService.DeleteMovie(id);
                return Ok("Movie deleted!");
            }
            catch (MovieDataException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Yikes, that's not good! :(");
            }
        }

        [HttpDelete("{id}")] //https://localhost:7118/api/Movies/1
        public IActionResult DeleteMovieRoute([FromRoute] int id)
        {
            try
            {
                _movieService.DeleteMovie(id);
                return Ok("Movie deleted!");
            }
            catch (MovieDataException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Yikes, that's not good! :(");
            }
        }

        [HttpPut] //https://localhost:7118/api/Movies/2
        public IActionResult UpdateMovie([FromBody] MovieUpdateDto movieUpdateDto)
        {
            try
            {
                _movieService.UpdateMovie(movieUpdateDto);
                return Ok("Movie updated successfully!");
            }
            catch (MovieDataException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Yikes, that's not good! :(");
            }
        }
    }
}