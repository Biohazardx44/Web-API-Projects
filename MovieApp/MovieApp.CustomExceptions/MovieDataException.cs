namespace MovieApp.CustomExceptions
{
    public class MovieDataException : Exception
    {
        public MovieDataException(string message) : base(message) { }
    }
}