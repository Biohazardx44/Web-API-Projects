using MovieApp.Domain.Enums;
using MovieApp.DTOs.MovieDTOs;

namespace MovieApp.Services.Abstraction
{
    public interface IMovieService
    {
        List<MovieDto> GetAllMovies(int userId);
        MovieDto GetById(int id);
        void AddMovie(AddMovieDto addMovieDto);
        void UpdateMovie(UpdateMovieDto updateMovieDto);
        void DeleteMovie(int id);
        List<MovieDto> FilterMovies(Genre? genre, int? year, int userId);
    }
}