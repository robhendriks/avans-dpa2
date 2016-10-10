using System;
using System.Linq;
using System.Collections.Generic;

namespace DPA_Musicsheets
{
    public class MusicNote
    {
        public double Length { get; private set; }
        public int Octave { get; private set; }
        public MusicNoteNote Note { get; private set; }
        public bool HasBarLine { get; set; }

        public MusicNote(int length, int octave, MusicNoteNote note)
        {
            Length = length;
            Octave = octave;
            Note = note;

            HasBarLine = false;
        }

        public override string ToString()
        {
            return "MusicNote{Length=" + Length
                + ", Octave=" + Octave
                + ", Note=" + Note
                + "}";
        }
    }
}
