using Microsoft.EntityFrameworkCore;
using MovieApp.DataAccess.Data;
using MovieApp.DataAccess.Repositories.Abstraction;
using MovieApp.Domain.Models;

namespace MovieApp.DataAccess.Repositories.Implementation.EntityFrameworkImplementation
{
    public class MovieRepositoryEntity : IMovieRepository
    {
        private readonly MovieAppDbContext _movieAppDbContext;

        public MovieRepositoryEntity(MovieAppDbContext movieAppDbContext)
        {
            _movieAppDbContext = movieAppDbContext;
        }

        /// <summary>
        /// Adds a new movie entity to the database using Entity Framework.
        /// </summary>
        /// <param name="entity">The movie entity to be added.</param>
        public void Add(Movie entity)
        {
            _movieAppDbContext.Movies.Add(entity);
            _movieAppDbContext.SaveChanges();
        }

        /// <summary>
        /// Deletes a movie entity from the database using Entity Framework.
        /// </summary>
        /// <param name="entity">The movie entity to be deleted.</param>
        public void Delete(Movie entity)
        {
            _movieAppDbContext.Movies.Remove(entity);
            _movieAppDbContext.SaveChanges();
        }

        /// <summary>
        /// Retrieves a list of all movie entities from the database, including related user information, using Entity Framework.
        /// </summary>
        /// <returns>A list of movie entities with user information.</returns>
        public List<Movie> GetAll()
        {
            return _movieAppDbContext.Movies
                    .Include(x => x.User).ToList();
        }

        /// <summary>
        /// Retrieves a movie entity by its unique identifier, including related user information, using Entity Framework.
        /// </summary>
        /// <param name="id">The unique identifier of the movie.</param>
        /// <returns>The movie entity with user information if found; otherwise, null.</returns>
        public Movie GetById(int id)
        {
            return _movieAppDbContext.Movies
                    .Include(x => x.User)
                    .SingleOrDefault(movie => movie.Id == id);
        }

        /// <summary>
        /// Updates an existing movie entity in the database using Entity Framework.
        /// </summary>
        /// <param name="entity">The movie entity to be updated.</param>
        public void Update(Movie entity)
        {
            _movieAppDbContext.Movies.Update(entity);
            _movieAppDbContext.SaveChanges();
        }
    }
}