using System;

namespace DPA_Musicsheets.Midi
{
    public class MidiLoadException : Exception
    {
        public MidiLoadException()
        {
        }

        public MidiLoadException(string message) : base(message)
        {
        }

        public MidiLoadException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
