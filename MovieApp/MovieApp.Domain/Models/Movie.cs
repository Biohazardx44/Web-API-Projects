using MovieApp.Domain.Enums;

namespace MovieApp.Domain.Models
{
    public class Movie : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Year { get; set; }
        public Genre Genre { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}