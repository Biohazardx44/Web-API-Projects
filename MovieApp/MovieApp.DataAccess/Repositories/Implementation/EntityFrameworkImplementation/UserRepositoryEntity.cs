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

        /// <summary>
        /// Adds a new user entity to the database using Entity Framework.
        /// </summary>
        /// <param name="entity">The user entity to be added.</param>
        public void Add(User entity)
        {
            _movieAppDbContext.Users.Add(entity);
            _movieAppDbContext.SaveChanges();
        }

        /// <summary>
        /// Deletes a user entity from the database using Entity Framework.
        /// </summary>
        /// <param name="entity">The user entity to be deleted.</param>
        public void Delete(User entity)
        {
            _movieAppDbContext.Users.Remove(entity);
            _movieAppDbContext.SaveChanges();
        }

        /// <summary>
        /// Retrieves a list of all user entities from the database using Entity Framework.
        /// </summary>
        /// <returns>A list of user entities.</returns>
        public List<User> GetAll()
        {
            return _movieAppDbContext.Users.ToList();
        }

        /// <summary>
        /// Retrieves a user entity by its unique identifier using Entity Framework.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>The user entity if found; otherwise, null.</returns>
        public User GetById(int id)
        {
            return _movieAppDbContext.Users
                    .SingleOrDefault(user => user.Id == id);
        }

        /// <summary>
        /// Retrieves a user entity by username using Entity Framework.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <returns>The user entity if found; otherwise, null.</returns>
        public User GetUserByUsername(string username)
        {
            return _movieAppDbContext.Users
                    .SingleOrDefault(user => user.Username == username);
        }

        /// <summary>
        /// Logs in a user by verifying the provided username and hashed password using Entity Framework.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="hashedPassword">The hashed password of the user.</param>
        /// <returns>The user entity if the login is successful; otherwise, null.</returns>
        public User LoginUser(string username, string hashedPassword)
        {
            return _movieAppDbContext.Users
                    .FirstOrDefault(user => user.Username.ToLower() == username.ToLower() && user.Password == hashedPassword);
        }

        /// <summary>
        /// Saves changes made to the database using Entity Framework.
        /// </summary>
        public void SaveChanges(User user)
        {
            _movieAppDbContext.SaveChanges();
        }

        /// <summary>
        /// Updates an existing user entity in the database using Entity Framework.
        /// </summary>
        /// <param name="entity">The user entity to be updated.</param>
        public void Update(User entity)
        {
            _movieAppDbContext.Users.Update(entity);
            _movieAppDbContext.SaveChanges();
        }
    }
}