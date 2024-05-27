using MovieApp.DataAccess.Data;
using MovieApp.DataAccess.Repositories.Abstraction;
using MovieApp.Domain.Models;

namespace MovieApp.DataAccess.Repositories.Implementation.EntityFrameworkImplementation
{
    public class UserRepositoryEntity : IUserRepository
    {
        private readonly MovieAppDbContext _movieAppDbContext;
        public UserRepositoryEntity(MovieAppDbContext movieAppDbContext)
        {
            _movieAppDbContext = movieAppDbContext;
        }

        public void Add(User entity)
        {
            _movieAppDbContext.Users.Add(entity);
            _movieAppDbContext.SaveChanges();
        }

        public void Delete(User entity)
        {
            _movieAppDbContext.Users.Remove(entity);
            _movieAppDbContext.SaveChanges();
        }

        public List<User> GetAll()
        {
            return _movieAppDbContext.Users.ToList();
        }

        public User GetById(int id)
        {
            return _movieAppDbContext.Users
            .SingleOrDefault(user => user.Id == id);
        }

        public User GetUserByUsername(string username)
        {
            return _movieAppDbContext.Users.SingleOrDefault(user => user.Username == username);
        }

        public User LoginUser(string username, string hashedPassword)
        {
            return _movieAppDbContext.Users.FirstOrDefault(user =>
            user.Username.ToLower() == username.ToLower() &&
            user.Password == hashedPassword);
        }

        public void SaveChanges()
        {
            _movieAppDbContext.SaveChanges();
        }

        public void Update(User entity)
        {
            _movieAppDbContext.Users.Update(entity);
            _movieAppDbContext.SaveChanges();
        }
    }
}