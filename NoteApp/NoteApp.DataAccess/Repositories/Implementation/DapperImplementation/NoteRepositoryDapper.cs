using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NoteApp.DataAccess.Repositories.Abstraction;
using NoteApp.Domain.Models;

namespace NoteApp.DataAccess.Repositories.Implementation.DapperImplementation
{
    public class NoteRepositoryDapper : INoteRepository
    {
        private readonly string _connectionString;

        public NoteRepositoryDapper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("NoteAppCS");
        }

        public void Add(Note entity)
        {
            using SqlConnection sqlConnection = new SqlConnection(_connectionString);
            var query = "EXEC dbo.SP_AddNote @Text, @Priority, @Tag, @UserId";
            sqlConnection.Execute(query, entity);
        }

        public void Delete(Note entity)
        {
            using SqlConnection sqlConnection = new SqlConnection(_connectionString);
            var query = "DELETE FROM Notes WHERE Id = @Id";
            sqlConnection.Execute(query, new { entity.Id });
        }

        public List<Note> GetAll()
        {
            using SqlConnection sqlConnection = new SqlConnection(_connectionString);
            var query = "SELECT * FROM dbo.Notes";
            return sqlConnection.Query<Note>(query).ToList();
        }

        public Note GetById(int id)
        {
            using SqlConnection sqlConnection = new SqlConnection(_connectionString);
            var query = "SELECT * FROM dbo.Notes WHERE Id = @noteId";
            return sqlConnection.QueryFirstOrDefault<Note>(query, new { noteId = id });
        }

        public void Update(Note entity)
        {
            using SqlConnection sqlConnection = new SqlConnection(_connectionString);
            var query = @"UPDATE dbo.Notes 
                        SET Text = @Text, Priority = @Priority, Tag = @Tag, UserId = @UserId
                        WHERE Id = @Id";
            sqlConnection.Execute(query, entity);
        }
    }
}