namespace NoteApp.CustomExceptions
{
    public class NoteNotFoundException : Exception
    {
        public NoteNotFoundException(string message) : base(message) { }
    }
}
