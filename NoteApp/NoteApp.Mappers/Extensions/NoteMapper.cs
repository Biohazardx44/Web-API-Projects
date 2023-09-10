using NoteApp.Domain.Models;
using NoteApp.DTOs.NoteDTOs;

namespace NoteApp.Mappers.Extensions
{
    public static class NoteMapper
    {
        public static NoteDto ToNoteDto(this Note note)
        {
            var noteDto = new NoteDto
            {
                Text = note.Text,
                Priority = note.Priority,
                Tag = note.Tag
            };

            if (note.User is not null)
            {
                noteDto.UserFullName = $"{note.User.FirstName} {note.User.LastName}";
            }

            return noteDto;
        }

        public static Note ToNote(this AddNoteDto addNoteDto)
        {
            return new Note
            {
                Text = addNoteDto.Text,
                Priority = addNoteDto.Priority,
                Tag = addNoteDto.Tag,
                UserId = addNoteDto.UserId
            };
        }

        public static void ToNoteFromUpdateNoteDto(this Note note, UpdateNoteDto updateNoteDto, User user)
        {
            note.Text = updateNoteDto.Text;
            note.Priority = updateNoteDto.Priority;
            note.Tag = updateNoteDto.Tag;
            note.UserId = updateNoteDto.UserId;
            note.User = user;
        }
    }
}