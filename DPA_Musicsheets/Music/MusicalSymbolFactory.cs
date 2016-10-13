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
                NoteStemDirection direction = determineDirection(baseNote, note);
                NoteBeamType beamType = isPair(previousNote, note, nextNote, baseNote);
                NoteTieType tieType = hasTie(previousNote, note, nextNote);
                return new Note(GetNote(note.Note), 0, note.Octave, GetDuration(note.Length), direction, tieType, new List<NoteBeamType>() { beamType });
            }
        }

        private static  NoteStemDirection determineDirection(MusicNote baseNote, MusicNote note)
        {
            NoteStemDirection direction = NoteStemDirection.Up;
            if (baseNote != null)
            {
                int baseOctave = baseNote.Octave;
                if (note.Octave <= baseOctave && LilyPond.LilyPondHelper.Compare(note.Note, MusicNoteNote.B) < 0)
                {
                    direction = NoteStemDirection.Down;
                }
            }
            return direction;
        }

        private static NoteTieType hasTie(MusicNote previousNote, MusicNote note, MusicNote nextNote)
        {
            NoteTieType tieType = NoteTieType.None;

            if (note.HasTie && !previousNote.HasTie)
            {
                tieType = NoteTieType.Start;
            }

            if (note.HasTie && nextNote.HasTie)
            {
                tieType = NoteTieType.StopAndStartAnother;
            }

            if (!note.HasTie && previousNote.HasTie)
            {
                tieType = NoteTieType.Stop;
            }

            return tieType;
        }


        public static NoteBeamType isPair(MusicNote previousNote, MusicNote currentNote, MusicNote nextNote, MusicNote baseNote)
        {
            NoteBeamType t = NoteBeamType.Single;
            if (currentNote.Length < 8) return t;
            var dir = determineDirection(baseNote, currentNote);

            if (currentNote.Length == nextNote.Length)
            {
                if (previousNote.Length != currentNote.Length)
                {
                    //if (dir != determineDirection(baseNote, previousNote) &&
                    //    dir == determineDirection(baseNote, nextNote))
                    //{
                        Debug.WriteLine("Start");
                        t = NoteBeamType.Start;
                    //}
                }
                else
                {
                    Debug.WriteLine("Continue");
                    t = NoteBeamType.Continue;
                }
            }

            if (currentNote.Length == previousNote.Length && currentNote.Length != nextNote.Length)
            {
                //if (determineDirection(baseNote, previousNote) == determineDirection(baseNote, currentNote) && determineDirection(baseNote, nextNote) == determineDirection(baseNote, currentNote))

                //if (determineDirection(baseNote, previousNote) == dir && 
                //    dir != determineDirection(baseNote, nextNote))
                //{
                    Debug.WriteLine("End");
                    t = NoteBeamType.End;
                //}
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
