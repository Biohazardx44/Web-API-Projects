using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NoteApp.DataAccess.Repositories.Abstraction;
using NoteApp.Domain.Enums;
using NoteApp.Domain.Models;

namespace NoteApp.DataAccess.Repositories.Implementation.AdoNetImplementation
{
    public class NoteRepositoryAdoNet : INoteRepository
    {
        private readonly string _connectionString;

        public NoteRepositoryAdoNet(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("NoteAppCS");
        }

        /// <summary>
        /// Adds a new note entity to the database using ADO.NET.
        /// </summary>
        /// <param name="entity">The note entity to be added.</param>
        public void Add(Note entity)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                var query = @"INSERT INTO dbo.Notes (Text, Priority, Tag, UserId)
                              VALUES (@text, @priority, @tag, @userId)";

                using SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlCommand.Parameters.AddWithValue("@text", entity.Text);
                sqlCommand.Parameters.AddWithValue("@priority", entity.Priority);
                sqlCommand.Parameters.AddWithValue("@tag", entity.Tag);
                sqlCommand.Parameters.AddWithValue("@userId", entity.UserId);

                var rowsAffected = sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Deletes a note entity from the database using ADO.NET.
        /// </summary>
        /// <param name="entity">The note entity to be deleted.</param>
        public void Delete(Note entity)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                var query = @"DELETE FROM dbo.Notes 
                              WHERE Id = @noteId";

                using SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlCommand.Parameters.AddWithValue("@noteId", entity.Id);

                var rowsAffected = sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Retrieves a list of all note entities from the database using ADO.NET.
        /// </summary>
        /// <returns>A list of note entities.</returns>
        public List<Note> GetAll()
        {
            var notes = new List<Note>();

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                var query = @"SELECT n.Id, n.Text, n.Priority, n.Tag, n.UserId, u.FirstName, u.LastName
                            FROM Notes n
                            LEFT JOIN Users u ON n.UserId = u.Id";

                using SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                using SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    var note = new Note()
                    {
                        Id = sqlDataReader.GetInt32(0),
                        Text = sqlDataReader.GetString(1),
                        Priority = (Priority)sqlDataReader.GetInt32(2),
                        Tag = (Tag)sqlDataReader.GetInt32(3),
                        UserId = sqlDataReader.GetInt32(4),
                        User = new User
                        {
                            FirstName = sqlDataReader.GetString(5),
                            LastName = sqlDataReader.GetString(6)
                        }
                    };
                    notes.Add(note);
                }
            }
            return notes;
        }

        /// <summary>
        /// Retrieves a note entity by its unique identifier from the database using ADO.NET.
        /// </summary>
        /// <param name="id">The unique identifier of the note.</param>
        /// <returns>The note entity if found; otherwise, null.</returns>
        public Note GetById(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                var query = @"SELECT n.Id, n.Text, n.Priority, n.Tag, n.UserId, u.FirstName, u.LastName
                            FROM Notes n
                            LEFT JOIN Users u ON n.UserId = u.Id
                            WHERE n.Id = @NoteIdentificator";

                using SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@NoteIdentificator", id);
                using SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                if (sqlDataReader.Read())
                {
                    return new Note()
                    {
                        Id = sqlDataReader.GetInt32(0),
                        Text = sqlDataReader.GetString(1),
                        Priority = (Priority)sqlDataReader.GetInt32(2),
                        Tag = (Tag)sqlDataReader.GetInt32(3),
                        UserId = sqlDataReader.GetInt32(4),
                        User = new User
                        {
                            FirstName = sqlDataReader.GetString(5),
                            LastName = sqlDataReader.GetString(6)
                        }
                    };
                }
                return null;
            }
        }

        /// <summary>
        /// Updates an existing note entity in the database using ADO.NET.
        /// </summary>
        /// <param name="entity">The note entity to be updated.</param>
        public void Update(Note entity)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                var query = @"UPDATE dbo.Notes 
                              SET Text = @text, Priority = @priority, Tag = @tag, UserId = @userId
                              WHERE Id = @noteId";

                using SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlCommand.Parameters.AddWithValue("@text", entity.Text);
                sqlCommand.Parameters.AddWithValue("@priority", entity.Priority);
                sqlCommand.Parameters.AddWithValue("@tag", entity.Tag);
                sqlCommand.Parameters.AddWithValue("@userId", entity.UserId);
                sqlCommand.Parameters.AddWithValue("@noteId", entity.Id);

                var rowsAffected = sqlCommand.ExecuteNonQuery();
            }
        }
    }
}