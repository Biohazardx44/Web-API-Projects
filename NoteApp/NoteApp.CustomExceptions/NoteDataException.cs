﻿namespace NoteApp.CustomExceptions
{
    public class NoteDataException : Exception
    {
        public NoteDataException(string message) : base(message) { }
    }
}