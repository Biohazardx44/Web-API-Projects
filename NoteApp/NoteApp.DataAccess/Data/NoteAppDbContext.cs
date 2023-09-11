using Microsoft.EntityFrameworkCore;
using NoteApp.CryptoService;
using NoteApp.Domain.Enums;
using NoteApp.Domain.Models;

namespace NoteApp.DataAccess.Data
{
    /// <summary>
    /// Represents the database context for the NoteApp application, responsible for interacting with the underlying database.
    /// </summary>
    public class NoteAppDbContext : DbContext
    {
        public NoteAppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Note> Notes { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // NOTE

            // Validations
            modelBuilder.Entity<Note>()
                .Property(x => x.Text)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Note>()
                .Property(x => x.Priority)
                .IsRequired();

            modelBuilder.Entity<Note>()
                .Property(x => x.Tag)
                .IsRequired();

            // Relations
            modelBuilder.Entity<Note>()
                .HasOne(x => x.User)
                .WithMany(x => x.Notes)
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
               .Property(x => x.Age)
               .IsRequired();

            // Seed initial data
            modelBuilder.Entity<User>()
                .HasData(new User
                {
                    Id = 1,
                    FirstName = "FirstName",
                    LastName = "LastName",
                    Username = "user1",
                    Password = StringHasher.Hash("user1"),
                    Age = 20,
                    Notes = new List<Note>()
                });

            modelBuilder.Entity<User>()
                .HasData(new User
                {
                    Id = 2,
                    FirstName = "FirstName",
                    LastName = "LastName",
                    Username = "user2",
                    Password = StringHasher.Hash("user2"),
                    Age = 22,
                    Notes = new List<Note>()
                });

            modelBuilder.Entity<Note>()
                .HasData(new Note
                {
                    Id = 1,
                    Text = "Some Text",
                    Priority = Priority.Low,
                    Tag = Tag.SocialLife,
                    UserId = 1
                });
        }
    }
}