using NoteApp.DTOs.NoteDTOs;

namespace NoteApp.Services.Abstraction
{
    public interface INoteService
    {
        List<NoteDto> GetAllNotes(int userId);
        NoteDto GetById(int id);
        void AddNote(AddNoteDto addNoteDto);
        void UpdateNote(UpdateNoteDto updateNoteDto);
        void DeleteNote(int id);
    }
}