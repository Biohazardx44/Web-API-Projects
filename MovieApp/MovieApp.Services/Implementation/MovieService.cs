using MovieApp.CustomExceptions;
using MovieApp.DataAccess.Repositories.Abstraction;
using MovieApp.Domain.Enums;
using MovieApp.DTOs.MovieDTOs;
using MovieApp.Mappers.Extensions;
using MovieApp.Services.Abstraction;

namespace MovieApp.Services.Implementation
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IUserRepository _userRepository;

        public MovieService(IMovieRepository movieRepository,
                            IUserRepository userRepository)
        {
            _movieRepository = movieRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Adds a new movie to the database.
        /// </summary>
        /// <param name="addMovieDto">The DTO containing movie information to be added.</param>
        public void AddMovie(AddMovieDto addMovieDto)
        {
            var userDb = _userRepository.GetById(addMovieDto.UserId);
            if (userDb is null)
            {
                throw new UserNotFoundException($"User with ID {addMovieDto.UserId} does not exist!");
            }

            ValidateRequiredFields(addMovieDto);

            var newMovieDb = addMovieDto.ToMovie();
            _movieRepository.Add(newMovieDb);
        }

        /// <summary>
        /// Deletes a movie from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the movie to delete.</param>
        public void DeleteMovie(int id)
        {
            var movieFromDb = _movieRepository.GetById(id);
            if (id <= 0)
            {
                throw new MovieDataException("The ID must not be negative!");
            }

            if (movieFromDb == null)
            {
                throw new MovieNotFoundException($"Movie with ID {id} does not exist!");
            }

            _movieRepository.Delete(movieFromDb);
        }

        /// <summary>
        /// Filters the movies based on the specified genre and/or year for a given user.
        /// </summary>
        /// <param name="genre">The genre to filter movies by (optional).</param>
        /// <param name="year">The year to filter movies by (optional).</param>
        /// <param name="userId">The ID of the user whose movies should be filtered.</param>
        /// <returns>A list of <see cref="MovieDto"/> objects that match the specified filters.</returns>
        public List<MovieDto> FilterMovies(Genre? genre, int? year, int userId)
        {
            ValidateRequiredFields(genre, year);

            var movies = GetAllMovies(userId);
            if (genre.HasValue)
            {
                movies = movies.Where(x => x.Genre == genre.Value).ToList();
            }

            if (year.HasValue)
            {
                movies = movies.Where(x => x.Year == year.Value).ToList();
            }

            if (movies.Count == 0)
            {
                throw new MovieNotFoundException("No movies match the specified filters :(");
            }

            return movies;
        }

        /// <summary>
        /// Retrieves a list of all movies for a specific user from the database.
        /// </summary>
        /// <param name="userId">The ID of the user whose movies are to be retrieved.</param>
        /// <returns>A list of movie DTOs.</returns>
        public List<MovieDto> GetAllMovies(int userId)
        {
            var moviesFromDb = _movieRepository.GetAll();

            var userMovies = moviesFromDb.Where(movie => movie.UserId == userId)
                               .Select(movie => movie.ToMovieDto()).ToList();

            if (moviesFromDb == null)
            {
                throw new MovieNotFoundException("No movies found :(");
            }

            if (userMovies.Count == 0)
            {
                throw new MovieNotFoundException($"No movies found for user with ID {userId}!");
            }

            return userMovies;
        }

        /// <summary>
        /// Retrieves a movie by its unique ID from the database.
        /// </summary>
        /// <param name="id">The unique ID of the movie to retrieve.</param>
        /// <returns>The movie DTO if found; otherwise, null.</returns>
        public MovieDto GetById(int id)
        {
            var movieFromDb = _movieRepository.GetById(id);
            if (id <= 0)
            {
                throw new MovieDataException("The ID must not be negative!");
            }

            if (movieFromDb is null)
            {
                throw new MovieNotFoundException($"Movie with ID {id} does not exist!");
            }

            return movieFromDb.ToMovieDto();
        }

        /// <summary>
        /// Updates an existing movie in the database.
        /// </summary>
        /// <param name="updateMovieDto">The DTO containing updated movie information.</param>
        public void UpdateMovie(UpdateMovieDto updateMovieDto)
        {
            var movieFromDb = _movieRepository.GetById(updateMovieDto.Id);
            if (updateMovieDto is null)
            {
                throw new MovieNotFoundException($"Movie with ID {updateMovieDto.Id} does not exist!");
            }

            var userDb = _userRepository.GetById(updateMovieDto.UserId);
            if (userDb is null)
            {
                throw new UserNotFoundException($"User with ID {updateMovieDto.UserId} does not exist!");
            }

            ValidateRequiredFields(updateMovieDto);

            movieFromDb.ToMovieFromUpdateMovieDto(updateMovieDto, userDb);
            _movieRepository.Update(movieFromDb);
        }

        /// <summary>
        /// Validates the required fields for adding a new movie.
        /// </summary>
        /// <param name="addMovieDto">The DTO containing movie information to validate.</param>
        private void ValidateRequiredFields(AddMovieDto addMovieDto)
        {
            if (string.IsNullOrEmpty(addMovieDto.Title))
            {
                throw new MovieDataException("Title is required field!");
            }

            if (addMovieDto.Year <= 0)
            {
                throw new MovieDataException("Year is required field!");
            }

            if (!Enum.IsDefined(typeof(Genre), addMovieDto.Genre))
            {
                throw new MovieDataException("Genre is required field!");
            }

            if (!string.IsNullOrEmpty(addMovieDto.Description) && addMovieDto.Description.Length > 250)
            {
                throw new MovieDataException("Description length should be a maximum of 250 characters!");
            }
        }

        /// <summary>
        /// Validates the required fields for updating a movie.
        /// </summary>
        /// <param name="updateMovieDto">The DTO containing updated movie information to validate.</param>
        private void ValidateRequiredFields(UpdateMovieDto updateMovieDto)
        {
            if (string.IsNullOrEmpty(updateMovieDto.Title))
            {
                throw new MovieDataException("Title is required field!");
            }

            if (updateMovieDto.Year <= 0)
            {
                throw new MovieDataException("Year is required field!");
            }

            if (!Enum.IsDefined(typeof(Genre), updateMovieDto.Genre))
            {
                throw new MovieDataException("Genre is required field!");
            }

            if (!string.IsNullOrEmpty(updateMovieDto.Description) && updateMovieDto.Description.Length > 250)
            {
                throw new MovieDataException("Description length should be a maximum of 250 characters!");
            }
        }

        /// <summary>
        /// Validates the required fields for filtering movies.
        /// </summary>
        /// <param name="genre">The genre to validate (optional).</param>
        /// <param name="year">The year to validate (optional).</param>
        private void ValidateRequiredFields(Genre? genre, int? year)
        {
            if (!genre.HasValue && !year.HasValue)
            {
                throw new MovieDataException("At least one filter parameter (genre or year) must be provided!");
            }

            if (genre.HasValue && !Enum.IsDefined(typeof(Genre), genre.Value))
            {
                throw new MovieDataException("Invalid genre value!");
            }

            if (year.HasValue && (year.Value < 1900 || year.Value > DateTime.Now.Year))
            {
                throw new MovieDataException("Invalid year value!");
            }
        }
    }
}