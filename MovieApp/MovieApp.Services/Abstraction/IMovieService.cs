using MovieApp.Domain.Enums;
using MovieApp.DTOs.MovieDTOs;

namespace MovieApp.Services.Abstraction
{
    public interface IMovieService
    {
        List<MovieDto> GetAllMovies(int userId);
        MovieDto GetById(int id);
        void AddMovie(MovieAddDto movieAddDto);
        void UpdateMovie(MovieUpdateDto movieUpdateDto);
        void DeleteMovie(int id);
        List<MovieDto> FilterMovies(Genre? genre, int? year, int userId);
    }
}