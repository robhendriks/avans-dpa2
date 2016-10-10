using System;
using System.Linq;
using System.Collections.Generic;

namespace DPA_Musicsheets
{
    public class MusicNote
    {
        public static readonly IEnumerable<MusicNoteNote> Notes = Enum.GetValues(typeof(MusicNoteNote)).Cast<MusicNoteNote>();

        public double Length { get; private set; }
        public int Octave { get; private set; }
        public MusicNoteNote Note { get; private set; }

        public MusicNote(int length, int keycode, MusicNoteNote note)
        {
            Length = length;
            Octave = GetOctave(keycode);
            Note = note;
        }

        public MusicNote(int length, int keycode)
            : this(length, keycode, GetNote(keycode))
        {
        }

        public static int GetOctave(int keycode)
        {
            return (keycode / 12) - 1;
        }

        public static MusicNoteNote GetNote(int keycode)
        {
            return Notes.ElementAt(keycode % 12);
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
