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

        public void Add(User entity)
        {
            using SqlConnection sqlConnection = new SqlConnection(_connectionString);
            var query = @"INSERT INTO Users (FirstName, LastName, Username, Password, Age)
                          VALUES (@FirstName, @LastName, @Username, @Password, @Age)";
            sqlConnection.Execute(query, entity);
        }

        public void Delete(User entity)
        {
            using SqlConnection sqlConnection = new SqlConnection(_connectionString);
            var query = "DELETE FROM Users WHERE Id = @Id";
            sqlConnection.Execute(query, new { entity.Id });
        }

        public List<User> GetAll()
        {
            using SqlConnection sqlConnection = new SqlConnection(_connectionString);
            var query = "SELECT * FROM Users";
            return sqlConnection.Query<User>(query).ToList();
        }

        public User GetById(int id)
        {
            using SqlConnection sqlConnection = new SqlConnection(_connectionString);
            var query = "SELECT * FROM Users WHERE Id = @Id";
            return sqlConnection.QueryFirstOrDefault<User>(query, new { Id = id });
        }

        public User GetUserByUsername(string username)
        {
            using SqlConnection sqlConnection = new SqlConnection(_connectionString);
            var query = "SELECT * FROM Users WHERE Username = @Username";
            return sqlConnection.QueryFirstOrDefault<User>(query, new { Username = username });
        }

        public User LoginUser(string username, string hashedPassword)
        {
            using SqlConnection sqlConnection = new SqlConnection(_connectionString);
            var query = @"SELECT * FROM Users 
                          WHERE Username = @Username AND Password = @Password";
            return sqlConnection.QueryFirstOrDefault<User>(query, new { Username = username, Password = hashedPassword });
        }

        public void SaveChanges() { }

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