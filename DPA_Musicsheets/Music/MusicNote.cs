﻿namespace DPA_Musicsheets
{
    public class MusicNote
    {
        public double Length { get; private set; }
        public int Octave { get; private set; }
        public MusicNoteNote Note { get; private set; }
        public bool IsRelative { get; set; }
        public bool HasBarLine { get; set; }
        public bool HasTie { get; set; }
        public bool HasLengthMultiplier { get; set; }

        public MusicNote(int length, int octave, MusicNoteNote note, bool isRelative = false)
        {
            Length = length;
            Octave = octave;
            Note = note;
            IsRelative = isRelative;
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
