using NoteApp.Domain.Enums;

namespace NoteApp.Domain.Models
{
    public class Note : BaseEntity
    {
        public string Text { get; set; } = string.Empty;
        public Priority Priority { get; set; }
        public Tag Tag { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}