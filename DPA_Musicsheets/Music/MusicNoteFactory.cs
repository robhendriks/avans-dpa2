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

            //double deltaTicks = nextEvent.AbsoluteTicks - currentEvent.AbsoluteTicks;
            //double percentageOfBeatNote = deltaTicks / sequence.Division;
            //double percentageOfWholeNote = (1.0 / staff.TimeSignature[1]) * percentageOfBeatNote;

            return new MusicNote(-999, message.Data1, message.Command);
        }
    }
}
