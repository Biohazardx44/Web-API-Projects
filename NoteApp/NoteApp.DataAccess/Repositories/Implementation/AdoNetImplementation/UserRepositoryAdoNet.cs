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

        public void SaveChanges()
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                using (SqlTransaction transaction = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

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