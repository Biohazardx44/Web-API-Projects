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

        /// <summary>
        /// Gets all movies for the authenticated user.
        /// </summary>
        /// <returns>A list of movie objects if found; otherwise, an error response.</returns>
        [HttpGet]
        public IActionResult GetAllMovies()
        {
            try
            {
                var userId = User.FindFirstValue("userId");

                return Ok(_movieService.GetAllMovies(int.Parse(userId)));
            }
            catch (MovieNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Yikes, that's not good! :(");
            }
        }

        /// <summary>
        /// Retrieves a movie by its unique identifier using the route parameter.
        /// </summary>
        /// <param name="id">The unique identifier of the movie.</param>
        /// <returns>The requested movie if found; otherwise, an error response.</returns>
        [HttpGet("{id}")]
        public IActionResult GetMovieByIdRoute([FromRoute] int id)
        {
            try
            {
                return Ok(_movieService.GetById(id));
            }
            catch (MovieDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (MovieNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Yikes, that's not good! :(");
            }
        }

        /// <summary>
        /// Retrieves a movie by its unique identifier using a query parameter.
        /// </summary>
        /// <param name="id">The unique identifier of the movie.</param>
        /// <returns>The requested movie if found; otherwise, an error response.</returns>
        [HttpGet("findById")]
        public IActionResult GetMovieByIdQuery([FromQuery] int id)
        {
            try
            {
                return Ok(_movieService.GetById(id));
            }
            catch (MovieDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (MovieNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Yikes, that's not good! :(");
            }
        }

        /// <summary>
        /// Filters movies based on the specified genre and/or year for the current user.
        /// </summary>
        /// <param name="genre">The genre to filter movies by (optional).</param>
        /// <param name="year">The year to filter movies by (optional).</param>
        /// <returns>A list of movies that match the specified filters if any; otherwise, an error response.</returns>
        [HttpGet("filter")]
        public IActionResult FilterMovies([FromQuery] Genre? genre, [FromQuery] int? year)
        {
            try
            {
                var userId = User.FindFirstValue("userId");

                return Ok(_movieService.FilterMovies(genre, year, int.Parse(userId)));
            }
            catch (MovieDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (MovieNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Yikes, that's not good! :(");
            }
        }

        /// <summary>
        /// Adds a new movie to the database.
        /// </summary>
        /// <param name="addMovieDto">The data for the new movie.</param>
        /// <returns>A success response if the movie is added; otherwise, an error response.</returns>
        [HttpPost]
        public IActionResult AddMovie([FromBody] AddMovieDto addMovieDto)
        {
            try
            {
                _movieService.AddMovie(addMovieDto);
                return StatusCode(StatusCodes.Status201Created, "Movie created!");
            }
            catch (MovieDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Yikes, that's not good! :(");
            }
        }

        /// <summary>
        /// Deletes a movie from the database by its ID provided in the request body.
        /// </summary>
        /// <param name="id">The unique identifier of the movie to delete, provided in the request body.</param>
        /// <returns>A success response if the movie is deleted; otherwise, an error response.</returns>
        [HttpDelete]
        public IActionResult DeleteMovieBody([FromBody] int id)
        {
            try
            {
                _movieService.DeleteMovie(id);
                return Ok("Movie deleted!");
            }
            catch (MovieDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (MovieNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Yikes, that's not good! :(");
            }
        }

        /// <summary>
        /// Deletes a movie from the database by its ID provided in the route.
        /// </summary>
        /// <param name="id">The unique identifier of the movie to delete, provided in the route.</param>
        /// <returns>A success response if the movie is deleted; otherwise, an error response.</returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteMovieRoute([FromRoute] int id)
        {
            try
            {
                _movieService.DeleteMovie(id);
                return Ok("Movie deleted!");
            }
            catch (MovieDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (MovieNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Yikes, that's not good! :(");
            }
        }

        /// <summary>
        /// Updates an existing movie in the database.
        /// </summary>
        /// <param name="updateMovieDto">The data for updating the movie.</param>
        /// <returns>A success response if the movie is updated; otherwise, an error response.</returns>
        [HttpPut]
        public IActionResult UpdateMovie([FromBody] UpdateMovieDto updateMovieDto)
        {
            try
            {
                _movieService.UpdateMovie(updateMovieDto);
                return Ok("Movie updated successfully!");
            }
            catch (MovieDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (MovieNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UserNotFoundException ex)
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