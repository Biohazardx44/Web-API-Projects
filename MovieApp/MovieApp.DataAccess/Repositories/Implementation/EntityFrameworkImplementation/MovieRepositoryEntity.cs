using MovieApp.DataAccess.Data;
using MovieApp.DataAccess.Repositories.Abstraction;
using MovieApp.Domain.Models;

namespace MovieApp.DataAccess.Repositories.Implementation.EntityFrameworkImplementation
{
    public class MovieRepositoryEntity : IRepository<Movie>
    {
        private readonly MovieAppDbContext _movieAppDbContext;

        public MovieRepositoryEntity(MovieAppDbContext movieAppDbContext)
        {
            _movieAppDbContext = movieAppDbContext;
        }

        public void Add(Movie entity)
        {
            _movieAppDbContext.Movies.Add(entity);
            _movieAppDbContext.SaveChanges();
        }

        public void Delete(Movie entity)
        {
            _movieAppDbContext.Movies.Remove(entity);
            _movieAppDbContext.SaveChanges();
        }

        public List<Movie> GetAll()
        {
            return _movieAppDbContext.Movies.ToList();
        }

        public Movie GetById(int id)
        {
            return _movieAppDbContext.Movies
            .SingleOrDefault(x => x.Id == id);
        }

        public void Update(Movie entity)
        {
            _movieAppDbContext.Movies.Update(entity);
            _movieAppDbContext.SaveChanges();
        }
    }
}