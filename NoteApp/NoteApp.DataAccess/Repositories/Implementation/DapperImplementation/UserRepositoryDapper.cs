using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NoteApp.DataAccess.Repositories.Abstraction;
using NoteApp.Domain.Models;

namespace NoteApp.DataAccess.Repositories.Implementation.DapperImplementation
{
    public class UserRepositoryDapper : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepositoryDapper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("NoteAppCS");
        }

        /// <summary>
        /// Adds a new user entity to the database using Dapper.
        /// </summary>
        /// <param name="entity">The user entity to be added.</param>
        public void Add(User entity)
        {
            using SqlConnection sqlConnection = new SqlConnection(_connectionString);
            var query = @"INSERT INTO Users (FirstName, LastName, Username, Password, Age)
                          VALUES (@FirstName, @LastName, @Username, @Password, @Age)";
            sqlConnection.Execute(query, entity);
        }

        /// <summary>
        /// Deletes a user entity from the database using Dapper.
        /// </summary>
        /// <param name="entity">The user entity to be deleted.</param>
        public void Delete(User entity)
        {
            using SqlConnection sqlConnection = new SqlConnection(_connectionString);
            var query = "DELETE FROM Users WHERE Id = @Id";
            sqlConnection.Execute(query, new { entity.Id });
        }

        /// <summary>
        /// Retrieves a list of all user entities from the database using Dapper.
        /// </summary>
        /// <returns>A list of user entities.</returns>
        public List<User> GetAll()
        {
            using SqlConnection sqlConnection = new SqlConnection(_connectionString);
            var query = "SELECT * FROM Users";
            return sqlConnection.Query<User>(query).ToList();
        }

        /// <summary>
        /// Retrieves a user entity by its unique identifier from the database using Dapper.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>The user entity if found; otherwise, null.</returns>
        public User GetById(int id)
        {
            using SqlConnection sqlConnection = new SqlConnection(_connectionString);
            var query = "SELECT * FROM Users WHERE Id = @Id";
            return sqlConnection.QueryFirstOrDefault<User>(query, new { Id = id });
        }

        /// <summary>
        /// Retrieves a user entity by its username from the database using Dapper.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <returns>The user entity if found; otherwise, null.</returns>
        public User GetUserByUsername(string username)
        {
            using SqlConnection sqlConnection = new SqlConnection(_connectionString);
            var query = "SELECT * FROM Users WHERE Username = @Username";
            return sqlConnection.QueryFirstOrDefault<User>(query, new { Username = username });
        }

        /// <summary>
        /// Logs in a user by verifying the username and hashed password using Dapper.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="hashedPassword">The hashed password of the user.</param>
        /// <returns>The user entity if the login is successful; otherwise, null.</returns>
        public User LoginUser(string username, string hashedPassword)
        {
            using SqlConnection sqlConnection = new SqlConnection(_connectionString);
            var query = @"SELECT * FROM Users 
                          WHERE Username = @Username AND Password = @Password";
            return sqlConnection.QueryFirstOrDefault<User>(query, new { Username = username, Password = hashedPassword });
        }

        /// <summary>
        /// Saves changes to the database using Dapper.
        /// </summary>
        public void SaveChanges(User user)
        {
            using SqlConnection sqlConnection = new SqlConnection(_connectionString);
            var query = "UPDATE Users SET Password = @Password WHERE Id = @Id";
            sqlConnection.Execute(query, new { Password = user.Password, Id = user.Id });
        }

        /// <summary>
        /// Updates an existing user entity in the database using Dapper.
        /// </summary>
        /// <param name="entity">The user entity to be updated.</param>
        public void Update(User entity)
        {
            using SqlConnection sqlConnection = new SqlConnection(_connectionString);
            var query = @"UPDATE Users 
                          SET FirstName = @FirstName, LastName = @LastName, 
                          Username = @Username, Password = @Password, Age = @Age
                          WHERE Id = @Id";
            sqlConnection.Execute(query, entity);
        }
    }
}