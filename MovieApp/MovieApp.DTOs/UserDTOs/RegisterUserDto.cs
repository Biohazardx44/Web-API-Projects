using MovieApp.Domain.Enums;

namespace MovieApp.DTOs.UserDTOs
{
    public class RegisterUserDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public Genre FavoriteGenre { get; set; }
    }
}