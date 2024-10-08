﻿using MovieApp.Domain.Enums;

namespace MovieApp.Domain.Models
{
    public class User : BaseEntity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public Genre FavoriteGenre { get; set; }
        public List<Movie> Movies { get; set; }
    }
}