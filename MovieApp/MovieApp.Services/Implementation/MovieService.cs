using MovieApp.CustomExceptions;
using MovieApp.DataAccess.Repositories.Abstraction;
using MovieApp.Domain.Enums;
using MovieApp.Domain.Models;
using MovieApp.DTOs.MovieDTOs;
using MovieApp.Mappers.Extensions;
using MovieApp.Services.Abstraction;

namespace MovieApp.Services.Implementation
{
    public class MovieService : IMovieService
    {
        private readonly IRepository<Movie> _movieRepository;

        public MovieService(IRepository<Movie> movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public void AddMovie(MovieAddDto movieAddDto)
        {
            if (string.IsNullOrEmpty(movieAddDto.Title))
            {
                throw new MovieDataException("Title is required field!");
            }

            if (movieAddDto.Year <= 0)
            {
                throw new MovieDataException("Year is required field!");
            }

            if (!Enum.IsDefined(typeof(Genre), movieAddDto.Genre))
            {
                throw new MovieDataException("Genre is required field!");
            }

            if (!string.IsNullOrEmpty(movieAddDto.Description) && movieAddDto.Description.Length > 250)
            {
                throw new MovieDataException("Description length should be a maximum of 250 characters!");
            }

            var newMovieDb = movieAddDto.MapToMovieAddDto();
            _movieRepository.Add(newMovieDb);
        }

        public void DeleteMovie(int id)
        {
            var movieFromDb = _movieRepository.GetById(id);

            if (id <= 0)
            {
                throw new MovieDataException("The id must not be negative!");
            }

            if (movieFromDb == null)
            {
                throw new MovieDataException($"Movie with id {id} does not exist");
            }

            _movieRepository.Delete(movieFromDb);
        }

        public List<MovieDto> FilterMovies(Genre? genre, int? year, int userId)
        {
            if (!genre.HasValue && !year.HasValue)
            {
                throw new MovieDataException("At least one filter parameter (genre or year) must be provided.");
            }

            if (genre.HasValue && !Enum.IsDefined(typeof(Genre), genre.Value))
            {
                throw new MovieDataException("Invalid genre value.");
            }

            if (year.HasValue && (year.Value < 1900 || year.Value > DateTime.Now.Year))
            {
                throw new MovieDataException("Invalid year value.");
            }

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
                throw new MovieDataException("No movies match the specified filters :(");
            }

            return movies;
        }

        public List<MovieDto> GetAllMovies(int userId)
        {
            var moviesFromDb = _movieRepository.GetAll();

            if (moviesFromDb == null)
            {
                throw new MovieDataException("No movies found :(");
            }

            return moviesFromDb.Where(x => x.UserId == userId)
                               .Select(x => x.MapToMovieDto()).ToList();
        }

        public MovieDto GetById(int id)
        {
            var movieFromDb = _movieRepository.GetById(id);

            if (id <= 0)
            {
                throw new MovieDataException("The id must not be negative!");
            }

            if (movieFromDb is null)
            {
                throw new MovieDataException($"Movie with id {id} does not exist");
            }

            return movieFromDb.MapToMovieDto();
        }

        public void UpdateMovie(MovieUpdateDto movieUpdateDto)
        {
            var movieFromDb = _movieRepository.GetById(movieUpdateDto.Id);

            if (movieUpdateDto.Id <= 0)
            {
                throw new MovieDataException("The id must not be negative!");
            }

            if (movieUpdateDto is null)
            {
                throw new MovieDataException($"Movie with id {movieUpdateDto.Id} does not exist");
            }

            if (string.IsNullOrEmpty(movieUpdateDto.Title))
            {
                throw new MovieDataException("Title is required field!");
            }

            if (movieUpdateDto.Year <= 0)
            {
                throw new MovieDataException("Year is required field!");
            }

            if (!Enum.IsDefined(typeof(Genre), movieUpdateDto.Genre))
            {
                throw new MovieDataException("Genre is required field!");
            }

            if (!string.IsNullOrEmpty(movieUpdateDto.Description) && movieUpdateDto.Description.Length > 250)
            {
                throw new MovieDataException("Description length should be a maximum of 250 characters!");
            }

            movieFromDb.UpdateMovieFromDto(movieUpdateDto);
            _movieRepository.Update(movieFromDb);
        }
    }
}