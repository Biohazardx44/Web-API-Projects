using NoteApp.CryptoService;
using NoteApp.DataAccess.Repositories.Abstraction;
using NoteApp.Domain.Models;

namespace NoteApp.Tests.FakeRepositories
{
    /// <summary>
    /// A fake repository implementation for user-related operations.
    /// </summary>
    public class FakeUserRepository : IUserRepository
    {
        private int userIdTracker = 2;
        private List<User> Users;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeUserRepository"/> class.
        /// </summary>
        public FakeUserRepository()
        {
            Users = new List<User>()
            {
                new User()
                {
                    Id = 1,
                    FirstName = "FirstName1",
                    LastName = "LastName1",
                    Username = "user1",
                    Age = 18,
                    Password = StringHasher.Hash("user1")
                },
                new User()
                {
                    Id = 2,
                    FirstName = "FirstName2",
                    LastName = "LastName2",
                    Username = "user2",
                    Age = 24,
                    Password = StringHasher.Hash("user2")
                }
            };
        }

        /// <summary>
        /// Adds a new user entity to the fake repository.
        /// </summary>
        /// <param name="entity">The user entity to be added.</param>
        public void Add(User entity)
        {
            entity.Id = ++userIdTracker;
            Users.Add(entity);
        }

        /// <summary>
        /// Deletes a user entity from the fake repository.
        /// </summary>
        /// <param name="entity">The user entity to be deleted.</param>
        public void Delete(User entity)
        {
            Users.Remove(entity);
        }

        /// <summary>
        /// Retrieves a list of all user entities from the fake repository.
        /// </summary>
        /// <returns>A list of user entities.</returns>
        public List<User> GetAll()
        {
            return Users;
        }

        /// <summary>
        /// Retrieves a user entity by its unique identifier from the fake repository.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>The user entity if found; otherwise, null.</returns>
        public User GetById(int id)
        {
            return Users
                .SingleOrDefault(user => user.Id == id);
        }

        /// <summary>
        /// Retrieves a user entity by username from the fake repository.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <returns>The user entity if found; otherwise, null.</returns>
        public User GetUserByUsername(string username)
        {
            return Users
                .SingleOrDefault(user => user.Username == username);
        }

        /// <summary>
        /// Logs in a user by verifying the provided username and hashed password from the fake repository.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="hashedPassword">The hashed password of the user.</param>
        /// <returns>The user entity if the login is successful; otherwise, null.</returns>
        public User LoginUser(string username, string hashedPassword)
        {
            return Users
                .FirstOrDefault(user => user.Username.ToLower() == username.ToLower() && user.Password == hashedPassword);
        }

        /// <summary>
        /// Saves changes made to the fake repository.
        /// </summary>
        public void SaveChanges(User user) { }

        /// <summary>
        /// Updates an existing user entity in the fake repository.
        /// </summary>
        /// <param name="entity">The user entity to be updated.</param>
        public void Update(User entity)
        {
            Users[Users.IndexOf(entity)] = entity;
        }
    }
}