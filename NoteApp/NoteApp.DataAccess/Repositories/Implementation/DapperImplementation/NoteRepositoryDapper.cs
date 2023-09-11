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

        /// <summary>
        /// Adds a new note entity to the database using Dapper.
        /// </summary>
        /// <param name="entity">The note entity to be added.</param>
        public void Add(Note entity)
        {
            using SqlConnection sqlConnection = new SqlConnection(_connectionString);
            var query = @"INSERT INTO dbo.Notes (Text, Priority, Tag, UserId)
                        VALUES (@Text, @Priority, @Tag, @UserId);
                        SELECT CAST(SCOPE_IDENTITY() AS INT);";

            var newNoteId = sqlConnection.ExecuteScalar<int>(query, entity);
            entity.Id = newNoteId;
        }

        /// <summary>
        /// Deletes a note entity from the database using Dapper.
        /// </summary>
        /// <param name="entity">The note entity to be deleted.</param>
        public void Delete(Note entity)
        {
            using SqlConnection sqlConnection = new SqlConnection(_connectionString);
            var query = "DELETE FROM Notes WHERE Id = @Id";
            sqlConnection.Execute(query, new { entity.Id });
        }

        /// <summary>
        /// Retrieves a list of all note entities from the database using Dapper.
        /// </summary>
        /// <returns>A list of note entities.</returns>
        public List<Note> GetAll()
        {
            using SqlConnection sqlConnection = new SqlConnection(_connectionString);
            var query = @"SELECT n.*, u.FirstName, u.LastName
                        FROM dbo.Notes n
                        LEFT JOIN dbo.Users u ON n.UserId = u.Id";

            var notes = sqlConnection.Query<Note, User, Note>(
                query,
                (note, user) =>
                {
                    note.User = user;
                    return note;
                },
                splitOn: "FirstName"
            ).ToList();

            return notes;
        }

        /// <summary>
        /// Retrieves a note entity by its unique identifier from the database using Dapper.
        /// </summary>
        /// <param name="id">The unique identifier of the note.</param>
        /// <returns>The note entity if found; otherwise, null.</returns>
        public Note GetById(int id)
        {
            using SqlConnection sqlConnection = new SqlConnection(_connectionString);
            var query = @"SELECT n.*, u.FirstName, u.LastName
                        FROM dbo.Notes n
                        LEFT JOIN dbo.Users u ON n.UserId = u.Id
                        WHERE n.Id = @noteId";

            var note = sqlConnection.Query<Note, User, Note>(
                query,
                (n, u) =>
                {
                    n.User = u;
                    return n;
                },
                new { noteId = id },
                splitOn: "FirstName"
            ).FirstOrDefault();

            return note;
        }

        /// <summary>
        /// Updates an existing note entity in the database using Dapper.
        /// </summary>
        /// <param name="entity">The note entity to be updated.</param>
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