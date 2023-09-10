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

        public List<Note> GetAll()
        {
            var notes = new List<Note>();

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                var query = @"SELECT n.Id, n.Text, n.Priority, n.Tag, n.UserId
                              FROM Notes n";

                using SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                using SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    var note = new Note()
                    {
                        Id = sqlDataReader.GetInt32(0),
                        Text = sqlDataReader.GetString(3),
                        Priority = (Priority)sqlDataReader.GetInt32(1),
                        Tag = (Tag)sqlDataReader["Tag"],
                        UserId = sqlDataReader.GetInt32(4)
                    };
                    notes.Add(note);
                }
            }
            return notes;
        }

        public Note GetById(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                var query = @$"SELECT n.Id, n.Text, n.Priority, n.Tag, n.UserId
                               FROM Notes n
                               WHERE n.Id = @NoteIdentificator";

                using SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@NoteIdentificator", id);
                using SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                if (sqlDataReader.Read())
                {
                    return new Note()
                    {
                        Id = sqlDataReader.GetInt32(0),
                        Text = sqlDataReader.GetString(3),
                        Priority = (Priority)sqlDataReader.GetInt32(1),
                        Tag = (Tag)sqlDataReader["Tag"],
                        UserId = sqlDataReader.GetInt32(4)
                    };
                }
                return null;
            }
        }

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