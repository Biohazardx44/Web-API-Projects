using Microsoft.EntityFrameworkCore;
using MovieApp.CryptoService;
using MovieApp.Domain.Enums;
using MovieApp.Domain.Models;

namespace MovieApp.DataAccess.Data
{
    /// <summary>
    /// Represents the database context for the MovieApp application, responsible for interacting with the underlying database.
    /// </summary>
    public class MovieAppDbContext : DbContext
    {
        public MovieAppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // MOVIE

            // Validations
            modelBuilder.Entity<Movie>()
                .Property(x => x.Title)
                .IsRequired();

            modelBuilder.Entity<Movie>()
                .Property(x => x.Description)
                .HasMaxLength(250);

            modelBuilder.Entity<Movie>()
                .Property(x => x.Year)
                .IsRequired();

            modelBuilder.Entity<Movie>()
                .Property(x => x.Genre)
                .IsRequired();

            // Relations
            modelBuilder.Entity<Movie>()
                .HasOne(x => x.User)
                .WithMany(x => x.Movies)
                .HasForeignKey(x => x.UserId);

            // USER

            // Validations
            modelBuilder.Entity<User>()
               .Property(x => x.FirstName)
               .HasMaxLength(50);

            modelBuilder.Entity<User>()
               .Property(x => x.LastName)
               .HasMaxLength(50);

            modelBuilder.Entity<User>()
               .Property(x => x.Username)
               .HasMaxLength(30)
               .IsRequired();

            modelBuilder.Entity<User>()
               .Property(x => x.Password)
               .HasMaxLength(30)
               .IsRequired();

            modelBuilder.Entity<User>()
               .Property(x => x.FavoriteGenre)
               .IsRequired();

            // Seed initial data
            modelBuilder.Entity<Movie>()
                .HasData(new Movie
                {
                    Id = 1,
                    Title = "Action Movie",
                    Description = "First Movie, Yay!",
                    Year = 1994,
                    Genre = Genre.Action,
                    UserId = 1
                });

            modelBuilder.Entity<Movie>()
                .HasData(new Movie
                {
                    Id = 2,
                    Title = "Thriller Movie",
                    Description = "Second Movie, Yay!",
                    Year = 1999,
                    Genre = Genre.Thriller,
                    UserId = 2
                });

            modelBuilder.Entity<Movie>()
                .HasData(new Movie
                {
                    Id = 3,
                    Title = "Better Action Movie",
                    Description = "Third Movie, Yay!",
                    Year = 2021,
                    Genre = Genre.Action,
                    UserId = 1
                });

            modelBuilder.Entity<Movie>()
                .HasData(new Movie
                {
                    Id = 4,
                    Title = "Comedy Movie",
                    Description = "Last Movie, Yay!",
                    Year = 1899,
                    Genre = Genre.Comedy,
                    UserId = 1
                });

            modelBuilder.Entity<User>()
                .HasData(new User
                {
                    Id = 1,
                    FirstName = "Eda",
                    LastName = "Nelson",
                    Username = "user1",
                    Password = StringHasher.Hash("user1"),
                    FavoriteGenre = Genre.Action,
                    Movies = new List<Movie>()
                });

            modelBuilder.Entity<User>()
                .HasData(new User
                {
                    Id = 2,
                    FirstName = "John",
                    LastName = "Peterson",
                    Username = "user2",
                    Password = StringHasher.Hash("user2"),
                    FavoriteGenre = Genre.Thriller,
                    Movies = new List<Movie>()
                });
        }
    }
}