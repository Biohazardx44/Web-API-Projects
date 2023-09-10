namespace NoteApp.DTOs.UserDTOs
{
    public class UpdateUserDetailsDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Username { get; set; } = string.Empty;
        public int Age { get; set; }
    }
}