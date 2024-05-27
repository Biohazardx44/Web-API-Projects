namespace MovieApp.CustomExceptions
{
    public class MovieDataException : Exception
    {
        public MovieDataException() : base("Generic Note Data exception occurred!") { }

        public MovieDataException(string message) : base(message) { }
    }
}