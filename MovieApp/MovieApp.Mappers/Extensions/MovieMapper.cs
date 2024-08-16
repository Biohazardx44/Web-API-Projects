using MovieApp.Domain.Models;
using MovieApp.DTOs.MovieDTOs;

namespace MovieApp.Mappers.Extensions
{
    public static class MovieMapper
    {
        public static MovieDto ToMovieDto(this Movie movie)
        {
            var movieDto = new MovieDto
            {
                Title = movie.Title,
                Description = movie.Description,
                Year = movie.Year,
                Genre = movie.Genre
            };

            if (movie.User is not null)
            {
                movieDto.UserFullName = $"{movie.User.FirstName} {movie.User.LastName}";
            }

            return movieDto;
        }

        public static Movie ToMovie(this AddMovieDto addMovieDto)
        {
            return new Movie
            {
                Title = addMovieDto.Title,
                Description = addMovieDto.Description,
                Year = addMovieDto.Year,
                Genre = addMovieDto.Genre,
                UserId = addMovieDto.UserId
            };
        }

        public static void ToMovieFromUpdateMovieDto(this Movie movie, UpdateMovieDto updateMovieDto, User user)
        {
            movie.Title = updateMovieDto.Title;
            movie.Description = updateMovieDto.Description;
            movie.Year = updateMovieDto.Year;
            movie.Genre = updateMovieDto.Genre;
            movie.UserId = updateMovieDto.UserId;
            movie.User = user;
        }
    }
}