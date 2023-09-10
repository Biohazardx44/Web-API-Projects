using Microsoft.EntityFrameworkCore;
using NoteApp.DataAccess.Data;
using NoteApp.DataAccess.Repositories.Abstraction;
using NoteApp.Domain.Models;

namespace NoteApp.DataAccess.Repositories.Implementation.EntityFrameworkImplementation
{
    public class NoteRepositoryEntity : INoteRepository
    {
        private readonly NoteAppDbContext _noteAppDbContext;

        public NoteRepositoryEntity(NoteAppDbContext noteAppDbContext)
        {
            _noteAppDbContext = noteAppDbContext;
        }

        public void Add(Note entity)
        {
            _noteAppDbContext.Notes.Add(entity);
            _noteAppDbContext.SaveChanges();
        }

        public void Delete(Note entity)
        {
            _noteAppDbContext.Notes.Remove(entity);
            _noteAppDbContext.SaveChanges();
        }

        public List<Note> GetAll()
        {
            return _noteAppDbContext.Notes
                    .Include(x => x.User).ToList();
        }

        public Note GetById(int id)
        {
            return _noteAppDbContext.Notes
                    .Include(x => x.User)
                    .SingleOrDefault(note => note.Id == id);
        }

        public void Update(Note entity)
        {
            _noteAppDbContext.Notes.Update(entity);
            _noteAppDbContext.SaveChanges();
        }
    }
}