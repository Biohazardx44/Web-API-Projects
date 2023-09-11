using NoteApp.DataAccess.Repositories.Abstraction;
using NoteApp.Domain.Enums;
using NoteApp.Domain.Models;

namespace NoteApp.Tests.FakeRepositories
{
    /// <summary>
    /// A fake repository implementation for note-related operations.
    /// </summary>
    public class FakeNoteRepository : INoteRepository
    {
        private int noteIdTracker = 2;
        private List<Note> Notes;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeNoteRepository"/> class.
        /// </summary>
        public FakeNoteRepository()
        {
            Notes = new List<Note>
            {
                new Note
                {
                    Id = 1,
                    Text = "Sample Note 1",
                    Priority = Priority.Medium,
                    Tag = Tag.Work,
                    UserId = 1
                },
                new Note
                {
                    Id = 2,
                    Text = "Sample Note 2",
                    Priority = Priority.High,
                    Tag = Tag.Health,
                    UserId = 1
                }
            };
        }

        /// <summary>
        /// Adds a new note entity to the fake repository.
        /// </summary>
        /// <param name="entity">The note entity to be added.</param>
        public void Add(Note entity)
        {
            entity.Id = ++noteIdTracker;
            Notes.Add(entity);
        }

        /// <summary>
        /// Deletes a note entity from the fake repository.
        /// </summary>
        /// <param name="entity">The note entity to be deleted.</param>
        public void Delete(Note entity)
        {
            Notes.Remove(entity);
        }

        /// <summary>
        /// Retrieves a list of all note entities from the fake repository.
        /// </summary>
        /// <returns>A list of note entities.</returns>
        public List<Note> GetAll()
        {
            return Notes;
        }

        /// <summary>
        /// Retrieves a note entity by its unique identifier from the fake repository.
        /// </summary>
        /// <param name="id">The unique identifier of the note.</param>
        /// <returns>The note entity if found; otherwise, null.</returns>
        public Note GetById(int id)
        {
            return Notes.SingleOrDefault(note => note.Id == id);
        }

        /// <summary>
        /// Updates an existing note entity in the fake repository.
        /// </summary>
        /// <param name="entity">The note entity to be updated.</param>
        public void Update(Note entity)
        {
            var existingNote = Notes.FirstOrDefault(note => note.Id == entity.Id);
            if (existingNote != null)
            {
                existingNote.Text = entity.Text;
                existingNote.Priority = entity.Priority;
                existingNote.Tag = entity.Tag;
                existingNote.UserId = entity.UserId;
            }
        }
    }
}