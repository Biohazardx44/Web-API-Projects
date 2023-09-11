using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NoteApp.DataAccess.Repositories.Abstraction;
using NoteApp.Domain.Models;

namespace NoteApp.DataAccess.Repositories.Implementation.AdoNetImplementation
{
    public class UserRepositoryAdoNet : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepositoryAdoNet(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("NoteAppCS");
        }

        /// <summary>
        /// Adds a new user entity to the database using ADO.NET.
        /// </summary>
        /// <param name="entity">The user entity to be added.</param>
        public void Add(User entity)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                var query = @"INSERT INTO dbo.Users (FirstName, LastName, Username, Password, Age)
                              VALUES (@firstName, @lastName, @username, @password, @age)";

                using SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlCommand.Parameters.AddWithValue("@firstName", entity.FirstName);
                sqlCommand.Parameters.AddWithValue("@lastName", entity.LastName);
                sqlCommand.Parameters.AddWithValue("@username", entity.Username);
                sqlCommand.Parameters.AddWithValue("@password", entity.Password);
                sqlCommand.Parameters.AddWithValue("@age", entity.Age);

                var rowsAffected = sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Deletes a user entity from the database using ADO.NET.
        /// </summary>
        /// <param name="entity">The user entity to be deleted.</param>
        public void Delete(User entity)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                var query = @"DELETE FROM dbo.Users 
                              WHERE Id = @userId";

                using SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlCommand.Parameters.AddWithValue("@userId", entity.Id);

                var rowsAffected = sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Retrieves a list of all user entities from the database using ADO.NET.
        /// </summary>
        /// <returns>A list of user entities.</returns>
        public List<User> GetAll()
        {
            var users = new List<User>();

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                var query = @"SELECT u.Id, u.FirstName, u.LastName, u.Username, u.Password, u.Age
                              FROM Users u";

                using SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                using SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    var user = new User()
                    {
                        Id = sqlDataReader.GetInt32(0),
                        FirstName = sqlDataReader.GetString(1),
                        LastName = sqlDataReader.GetString(2),
                        Username = sqlDataReader.GetString(3),
                        Password = sqlDataReader.GetString(4),
                        Age = sqlDataReader.GetInt32(5)
                    };
                    users.Add(user);
                }
            }
            return users;
        }

        /// <summary>
        /// Retrieves a user entity by its unique identifier from the database using ADO.NET.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>The user entity if found; otherwise, null.</returns>
        public User GetById(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                var query = @"SELECT u.Id, u.FirstName, u.LastName, u.Username, u.Password, u.Age
                              FROM Users u
                              WHERE u.Id = @userId";

                using SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@userId", id);
                using SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                if (sqlDataReader.Read())
                {
                    return new User()
                    {
                        Id = sqlDataReader.GetInt32(0),
                        FirstName = sqlDataReader.GetString(1),
                        LastName = sqlDataReader.GetString(2),
                        Username = sqlDataReader.GetString(3),
                        Password = sqlDataReader.GetString(4),
                        Age = sqlDataReader.GetInt32(5)
                    };
                }
                return null;
            }
        }

        /// <summary>
        /// Retrieves a user entity by its username from the database using ADO.NET.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <returns>The user entity if found; otherwise, null.</returns>
        public User GetUserByUsername(string username)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                var query = @"SELECT u.Id, u.FirstName, u.LastName, u.Username, u.Password, u.Age
                              FROM Users u
                              WHERE u.Username = @username";

                using SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@username", username);
                using SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                if (sqlDataReader.Read())
                {
                    return new User()
                    {
                        Id = sqlDataReader.GetInt32(0),
                        FirstName = sqlDataReader.GetString(1),
                        LastName = sqlDataReader.GetString(2),
                        Username = sqlDataReader.GetString(3),
                        Password = sqlDataReader.GetString(4),
                        Age = sqlDataReader.GetInt32(5)
                    };
                }
                return null;
            }
        }

        /// <summary>
        /// Logs in a user by verifying the username and hashed password using ADO.NET.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="hashedPassword">The hashed password of the user.</param>
        /// <returns>The user entity if the login is successful; otherwise, null.</returns>
        public User LoginUser(string username, string hashedPassword)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                var query = @"SELECT u.Id, u.FirstName, u.LastName, u.Username, u.Password, u.Age
                              FROM Users u
                              WHERE u.Username = @username AND u.Password = @password";

                using SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@username", username);
                sqlCommand.Parameters.AddWithValue("@password", hashedPassword);
                using SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                if (sqlDataReader.Read())
                {
                    return new User()
                    {
                        Id = sqlDataReader.GetInt32(0),
                        FirstName = sqlDataReader.GetString(1),
                        LastName = sqlDataReader.GetString(2),
                        Username = sqlDataReader.GetString(3),
                        Password = sqlDataReader.GetString(4),
                        Age = sqlDataReader.GetInt32(5)
                    };
                }
                return null;
            }
        }

        /// <summary>
        /// Saves changes to the database using ADO.NET.
        /// </summary>
        public void SaveChanges(User user)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var transaction = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        var query = "UPDATE Users SET Password = @Password WHERE Id = @Id";
                        using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection, transaction))
                        {
                            sqlCommand.Parameters.AddWithValue("@Password", user.Password);
                            sqlCommand.Parameters.AddWithValue("@Id", user.Id);
                            sqlCommand.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Updates an existing user entity in the database using ADO.NET.
        /// </summary>
        /// <param name="entity">The user entity to be updated.</param>
        public void Update(User entity)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                var query = @"UPDATE dbo.Users 
                              SET FirstName = @firstName, LastName = @lastName, Username = @username, Password = @password, Age = @age
                              WHERE Id = @userId";

                using SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlCommand.Parameters.AddWithValue("@firstName", entity.FirstName);
                sqlCommand.Parameters.AddWithValue("@lastName", entity.LastName);
                sqlCommand.Parameters.AddWithValue("@username", entity.Username);
                sqlCommand.Parameters.AddWithValue("@password", entity.Password);
                sqlCommand.Parameters.AddWithValue("@age", entity.Age);
                sqlCommand.Parameters.AddWithValue("@userId", entity.Id);

                var rowsAffected = sqlCommand.ExecuteNonQuery();
            }
        }
    }
}