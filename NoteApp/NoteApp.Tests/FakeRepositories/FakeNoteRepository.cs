using NoteApp.DataAccess.Repositories.Abstraction;
using NoteApp.Domain.Enums;
using NoteApp.Domain.Models;

namespace NoteApp.Tests.FakeRepositories
{
    public class FakeNoteRepository : INoteRepository
    {
        private int noteIdTracker = 2;
        private List<Note> Notes;

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

        public void Add(Note entity)
        {
            entity.Id = ++noteIdTracker;
            Notes.Add(entity);
        }

        public void Delete(Note entity)
        {
            Notes.Remove(entity);
        }

        public List<Note> GetAll()
        {
            return Notes;
        }

        public Note GetById(int id)
        {
            return Notes.SingleOrDefault(note => note.Id == id);
        }

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