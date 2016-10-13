using PSAMControlLibrary;
using System.Collections.Generic;
using System.Diagnostics;

namespace DPA_Musicsheets.Utility
{
    public static class MusicalSymbolFactory
    {
        private static readonly Dictionary<MusicNoteNote, string> NoteMap = new Dictionary<MusicNoteNote, string>
        {
            { MusicNoteNote.C, "C" }, { MusicNoteNote.CSharp, "C#" },
            { MusicNoteNote.D, "D" }, { MusicNoteNote.DSharp, "D#" },
            { MusicNoteNote.E, "E" },
            { MusicNoteNote.F, "F" }, { MusicNoteNote.FSharp, "F#" },
            { MusicNoteNote.G, "G" }, { MusicNoteNote.GSharp, "G#" },
            { MusicNoteNote.A, "A" }, { MusicNoteNote.ASharp, "A#" },
            { MusicNoteNote.B, "B" }
        };

        private static readonly Dictionary<double, MusicalSymbolDuration> DurationMap = new Dictionary<double, MusicalSymbolDuration>
        {
            { 128, MusicalSymbolDuration.d128th },
            { 64, MusicalSymbolDuration.d64th },
            { 32, MusicalSymbolDuration.d32nd },
            { 16, MusicalSymbolDuration.Sixteenth },
            { 8, MusicalSymbolDuration.Eighth },
            { 4, MusicalSymbolDuration.Quarter },
            { 2, MusicalSymbolDuration.Half },
            { 1, MusicalSymbolDuration.Whole }
        };

        public static MusicalSymbol Create(MusicNote baseNote, MusicNote note, MusicNote nextNote, MusicNote previousNote)
        {
            if (note.Note == MusicNoteNote.Rest)
            {
                return new Rest(GetDuration(note.Length));
            }
            else
            {
                NoteStemDirection direction = NoteStemDirection.Up;
                NoteTieType tieType = isPair(previousNote, note, nextNote);
                if (baseNote != null)
                {
                    int baseOctave = baseNote.Octave;
                    if (note.Octave <= baseOctave && LilyPond.LilyPondHelper.Compare(note.Note, MusicNoteNote.B) < 0)
                    {
                        direction = NoteStemDirection.Down;
                    }
                }

                return new Note(GetNote(note.Note), 0, note.Octave, GetDuration(note.Length), direction, tieType, new List<NoteBeamType>() { NoteBeamType.Single });
            }
        }

        public static NoteTieType isPair(MusicNote previousNote, MusicNote currentNote, MusicNote nextNote)
        {
            NoteTieType t = NoteTieType.None;
            if (currentNote.Octave == 8 && nextNote.Octave == 8)
            {
                if (previousNote.Octave == 8)
                {
                    t = NoteTieType.StopAndStartAnother;
                }
                else
                {
                    t = NoteTieType.Start;
                }
            }

            if (currentNote.Octave == 8 && previousNote.Octave == 8 && nextNote.Octave != 8)
            {
                t = NoteTieType.Stop;
            }

                return t;
        }

        public static string GetNote(MusicNoteNote note)
        {
            return NoteMap[note];
        }

        public static MusicalSymbolDuration GetDuration(double length)
        {
            return DurationMap[length];
        }
    }
}
