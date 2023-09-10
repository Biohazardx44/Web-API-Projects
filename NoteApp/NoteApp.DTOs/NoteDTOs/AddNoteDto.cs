using NoteApp.Domain.Enums;

namespace NoteApp.DTOs.NoteDTOs
{
    public class AddNoteDto
    {
        public string Text { get; set; } = string.Empty;
        public Priority Priority { get; set; }
        public Tag Tag { get; set; }
        public int UserId { get; set; }
    }
}