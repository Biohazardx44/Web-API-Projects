using NoteApp.CustomExceptions;
using NoteApp.DataAccess.Repositories.Abstraction;
using NoteApp.Domain.Enums;
using NoteApp.DTOs.NoteDTOs;
using NoteApp.Mappers.Extensions;
using NoteApp.Services.Abstraction;

namespace NoteApp.Services.Implementation
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _noteRepository;
        private readonly IUserRepository _userRepository;

        public NoteService(INoteRepository noteRepository,
                           IUserRepository userRepository)
        {
            _noteRepository = noteRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Adds a new note to the database.
        /// </summary>
        /// <param name="addNoteDto">The DTO containing note information to be added.</param>
        public void AddNote(AddNoteDto addNoteDto)
        {
            var userDb = _userRepository.GetById(addNoteDto.UserId);
            if (userDb is null)
            {
                throw new UserNotFoundException($"User with ID {addNoteDto.UserId} does not exist!");
            }

            ValidateRequiredFields(addNoteDto);

            var newNoteDb = addNoteDto.ToNote();
            _noteRepository.Add(newNoteDb);
        }

        /// <summary>
        /// Deletes a note from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the note to delete.</param>
        public void DeleteNote(int id)
        {
            var noteFromDb = _noteRepository.GetById(id);
            if (noteFromDb == null)
            {
                throw new NoteNotFoundException($"Note with ID {id} does not exist!");
            }

            _noteRepository.Delete(noteFromDb);
        }

        /// <summary>
        /// Retrieves a list of all notes for a specific user from the database.
        /// </summary>
        /// <param name="userId">The ID of the user whose notes are to be retrieved.</param>
        /// <returns>A list of note DTOs.</returns>
        public List<NoteDto> GetAllNotes(int userId)
        {
            var notesFromDb = _noteRepository.GetAll();

            var userNotes = notesFromDb.Where(note => note.UserId == userId)
                              .Select(note => note.ToNoteDto()).ToList();

            if (userNotes.Count == 0)
            {
                throw new NoteNotFoundException($"No notes found for user with ID {userId}!");
            }

            return userNotes;
        }

        /// <summary>
        /// Retrieves a note by its unique ID from the database.
        /// </summary>
        /// <param name="id">The unique ID of the note to retrieve.</param>
        /// <returns>The note DTO if found; otherwise, null.</returns>
        public NoteDto GetById(int id)
        {
            var noteFromDb = _noteRepository.GetById(id);
            if (noteFromDb is null)
            {
                throw new NoteNotFoundException($"Note with ID {id} does not exist!");
            }

            return noteFromDb.ToNoteDto();
        }

        /// <summary>
        /// Updates an existing note in the database.
        /// </summary>
        /// <param name="updateNoteDto">The DTO containing updated note information.</param>
        public void UpdateNote(UpdateNoteDto updateNoteDto)
        {
            var noteFromDb = _noteRepository.GetById(updateNoteDto.Id);
            if (noteFromDb is null)
            {
                throw new NoteNotFoundException($"Note with ID {updateNoteDto.Id} does not exist!");
            }

            var userDb = _userRepository.GetById(updateNoteDto.UserId);
            if (userDb is null)
            {
                throw new UserNotFoundException($"User with ID {updateNoteDto.UserId} does not exist!");
            }

            ValidateRequiredFields(updateNoteDto);

            noteFromDb.ToNoteFromUpdateNoteDto(updateNoteDto, userDb);
            _noteRepository.Update(noteFromDb);
        }

        /// <summary>
        /// Validates the required fields for adding a new note.
        /// </summary>
        /// <param name="addNoteDto">The DTO containing note information to validate.</param>
        private void ValidateRequiredFields(AddNoteDto addNoteDto)
        {
            if (!Enum.IsDefined(typeof(Priority), addNoteDto.Priority))
            {
                throw new NoteDataException("Priority is a required field!");
            }

            if (!Enum.IsDefined(typeof(Tag), addNoteDto.Tag))
            {
                throw new NoteDataException("Tag is a required field!");
            }

            if (string.IsNullOrEmpty(addNoteDto.Text))
            {
                throw new NoteDataException("Text is a required field!");
            }

            if (addNoteDto.Text.Length > 100)
            {
                throw new NoteDataException("Text cannot contain more than 100 characters!");
            }
        }

        /// <summary>
        /// Validates the required fields for updating a note.
        /// </summary>
        /// <param name="updateNoteDto">The DTO containing updated note information to validate.</param>
        private void ValidateRequiredFields(UpdateNoteDto updateNoteDto)
        {
            if (!Enum.IsDefined(typeof(Priority), updateNoteDto.Priority))
            {
                throw new NoteDataException("Priority is a required field!");
            }

            if (!Enum.IsDefined(typeof(Tag), updateNoteDto.Tag))
            {
                throw new NoteDataException("Tag is a required field!");
            }

            if (string.IsNullOrEmpty(updateNoteDto.Text))
            {
                throw new NoteDataException("Text is a required field!");
            }

            if (updateNoteDto.Text.Length > 100)
            {
                throw new NoteDataException("Text cannot contain more than 100 characters!");
            }
        }
    }
}