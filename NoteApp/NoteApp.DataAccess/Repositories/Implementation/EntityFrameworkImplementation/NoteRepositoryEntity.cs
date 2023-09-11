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

        /// <summary>
        /// Adds a new note entity to the database using Entity Framework.
        /// </summary>
        /// <param name="entity">The note entity to be added.</param>
        public void Add(Note entity)
        {
            _noteAppDbContext.Notes.Add(entity);
            _noteAppDbContext.SaveChanges();
        }

        /// <summary>
        /// Deletes a note entity from the database using Entity Framework.
        /// </summary>
        /// <param name="entity">The note entity to be deleted.</param>
        public void Delete(Note entity)
        {
            _noteAppDbContext.Notes.Remove(entity);
            _noteAppDbContext.SaveChanges();
        }

        /// <summary>
        /// Retrieves a list of all note entities from the database, including related user information, using Entity Framework.
        /// </summary>
        /// <returns>A list of note entities with user information.</returns>
        public List<Note> GetAll()
        {
            return _noteAppDbContext.Notes
                    .Include(x => x.User).ToList();
        }

        /// <summary>
        /// Retrieves a note entity by its unique identifier, including related user information, using Entity Framework.
        /// </summary>
        /// <param name="id">The unique identifier of the note.</param>
        /// <returns>The note entity with user information if found; otherwise, null.</returns>
        public Note GetById(int id)
        {
            return _noteAppDbContext.Notes
                    .Include(x => x.User)
                    .SingleOrDefault(note => note.Id == id);
        }

        /// <summary>
        /// Updates an existing note entity in the database using Entity Framework.
        /// </summary>
        /// <param name="entity">The note entity to be updated.</param>
        public void Update(Note entity)
        {
            _noteAppDbContext.Notes.Update(entity);
            _noteAppDbContext.SaveChanges();
        }
    }
}