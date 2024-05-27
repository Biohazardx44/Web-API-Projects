using MovieApp.Domain.Enums;

namespace MovieApp.DTOs.MovieDTOs
{
    public class MovieDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Year { get; set; }
        public Genre Genre { get; set; }
    }
}