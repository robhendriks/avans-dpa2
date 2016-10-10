using System;
using System.Collections.Generic;
using System.Linq;

namespace DPA_Musicsheets.Midi
{
    public static class MidiHelper
    {
        public static readonly IEnumerable<MusicNoteNote> Notes = Enum.GetValues(typeof(MusicNoteNote)).Cast<MusicNoteNote>();

        public static int GetOctave(int keycode)
        {
            return (keycode / 12) - 1;
        }

        public static MusicNoteNote GetNoteNote(int keycode)
        {
            return Notes.ElementAt(keycode % 12);
        }
    }
}
