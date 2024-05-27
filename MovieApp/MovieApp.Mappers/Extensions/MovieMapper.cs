using MovieApp.Domain.Models;
using MovieApp.DTOs.MovieDTOs;

namespace MovieApp.Mappers.Extensions
{
    public static class MovieMapper
    {
        public static MovieDto MapToMovieDto(this Movie movie)
        {
            return new MovieDto
            {
                Description = movie.Description,
                Genre = movie.Genre,
                Year = movie.Year,
                Title = movie.Title
            };
        }

        public static Movie MapToMovieAddDto(this MovieAddDto movieAddDto)
        {
            return new Movie
            {
                Description = movieAddDto.Description,
                Genre = movieAddDto.Genre,
                Year = movieAddDto.Year,
                Title = movieAddDto.Title
            };
        }

        public static void UpdateMovieFromDto(this Movie movie, MovieUpdateDto updateDto)
        {
            movie.Id = updateDto.Id;
            movie.Title = updateDto.Title;
            movie.Description = updateDto.Description;
            movie.Year = updateDto.Year;
            movie.Genre = updateDto.Genre;
        }
    }
}