using Sanford.Multimedia.Midi;
using System;

namespace DPA_Musicsheets
{
    public static class MusicNoteFactory
    {
        public static MusicNote Create(ChannelMessage message, MidiEvent currentEvent, MidiEvent nextEvent)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            if (currentEvent == null)
            {
                throw new ArgumentNullException(nameof(currentEvent));
            }
            if (nextEvent == null)
            {
                throw new ArgumentNullException(nameof(nextEvent));
            }
            return new MusicNote(-999, message.Data1, message.Command);
        }
    }
}
