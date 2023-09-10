using NoteApp.Domain.Enums;

namespace NoteApp.DTOs.NoteDTOs
{
    public class NoteDto
    {
        public string Text { get; set; } = string.Empty;
        public Priority Priority { get; set; }
        public Tag Tag { get; set; }
        public string UserFullName { get; set; } = string.Empty;
    }
}