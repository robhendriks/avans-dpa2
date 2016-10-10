using System;

namespace DPA_Musicsheets.LilyPond
{
    public class LilyPondException : Exception
    {
        public LilyPondException()
        {
        }

        public LilyPondException(string message) : base(message)
        {
        }

        public LilyPondException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
