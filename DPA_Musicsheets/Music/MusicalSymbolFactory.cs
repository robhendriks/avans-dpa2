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

        public static MusicalSymbol test(int i)
        {
            if(i == 0)
            {
                return new Note("B", 0, 4, MusicalSymbolDuration.Eighth, NoteStemDirection.Up, NoteTieType.None, new List<NoteBeamType>() { NoteBeamType.Start });
            }
            if (i == 2)
            {
                return new Note("B", 0, 4, MusicalSymbolDuration.Eighth, NoteStemDirection.Up, NoteTieType.None, new List<NoteBeamType>() { NoteBeamType.Continue });
            }
            if (i == 3)
            {
                return new Note("B", 0, 4, MusicalSymbolDuration.Eighth, NoteStemDirection.Up, NoteTieType.None, new List<NoteBeamType>() { NoteBeamType.End });
            }
            else
            {
                return new Note("B", 0, 4, MusicalSymbolDuration.Eighth, NoteStemDirection.Up, NoteTieType.Start, new List<NoteBeamType>() { NoteBeamType.Start });
            }
        }

        public static MusicalSymbol Create(MusicNote baseNote, MusicNote note, MusicNote nextNote, MusicNote previousNote)
        {
            if (note.Note == MusicNoteNote.Rest)
            {
                return new Rest(GetDuration(note.Length));
            }
            else
            {
                NoteStemDirection direction = NoteStemDirection.Up;
                NoteBeamType beamType = isPair(previousNote, note, nextNote);
                if (baseNote != null)
                {
                    int baseOctave = baseNote.Octave;
                    if (note.Octave <= baseOctave && LilyPond.LilyPondHelper.Compare(note.Note, MusicNoteNote.B) < 0)
                    {
                        direction = NoteStemDirection.Down;
                    }
                }

                return new Note(GetNote(note.Note), 0, note.Octave, GetDuration(note.Length), direction, NoteTieType.Start, new List<NoteBeamType>() { beamType });
            }
        }

        public static NoteBeamType isPair(MusicNote previousNote, MusicNote currentNote, MusicNote nextNote)
        {
            // Debug.WriteLine("PREVIOUS: " + previousNote.Length + "\t CURRENT: " + currentNote.Length + "\t NEXT: "+ nextNote.Length);

            NoteBeamType t = NoteBeamType.Single;

            if (currentNote.Length < 8) return t;

            if (currentNote.Length == nextNote.Length)
            {
                
                if (previousNote.Length == currentNote.Length)
                {
                    t = NoteBeamType.Continue;
                }
                else
                {
                    t = NoteBeamType.Start;
                }
            }

            if (currentNote.Length ==  previousNote.Length && currentNote.Length != nextNote.Length)
            {
                t = NoteBeamType.End;
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
