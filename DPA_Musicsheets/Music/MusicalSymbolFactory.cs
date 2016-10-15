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

        private static NoteBeamType lastState = NoteBeamType.Single;

        public static MusicalSymbol Create(MusicNote baseNote, MusicNote previousNote, MusicNote note, MusicNote nextNote)
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

                Note n = new Note(GetNote(note.Note), 0, note.Octave, GetDuration(note.Length), direction, tieType, new List<NoteBeamType>() { beamType });

                if (note.HasLengthMultiplier)
                {
                    n.NumberOfDots = 1;
                }

                return n;
            }
        }

        private static NoteTieType hasTie(MusicNote previousNote, MusicNote note, MusicNote nextNote)
        {
            NoteTieType tieType = NoteTieType.None;
            if (previousNote == null || nextNote == null)
            {
                return tieType;
            }

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

        private static NoteStemDirection determineDirection(MusicNote baseNote, MusicNote note)
        {
            NoteStemDirection direction = NoteStemDirection.Down;
            if (baseNote == null || note == null)
            {
                return direction;
            }

            int baseOctave = baseNote.Octave;
            if (note.Octave <= baseOctave && LilyPond.LilyPondHelper.Compare(note.Note, MusicNoteNote.B) < 0)
            {
                direction = NoteStemDirection.Up;
            }

            return direction;
        }

        public static NoteBeamType isPair(MusicNote previousNote, MusicNote currentNote, MusicNote nextNote, MusicNote baseNote)
        {

            NoteBeamType result = NoteBeamType.Single;
            if (currentNote.Length < 8)
            {
                return result;
            }

            var previousDirection = determineDirection(baseNote, previousNote);
            var currentDirection = determineDirection(baseNote, currentNote);
            var nextDirection = determineDirection(baseNote, nextNote);

            if (nextNote != null && 
                currentNote.Length == nextNote.Length &&
                currentDirection == nextDirection &&
                (lastState == NoteBeamType.Single || lastState == NoteBeamType.End))
            {
                //Debug.WriteLine("START");
                result = NoteBeamType.Start;
                lastState = result;
                return result;
            }

            if (previousNote != null && nextNote != null &&
                currentNote.Length == previousNote.Length && currentNote.Length == nextNote.Length &&
                currentDirection == previousDirection && currentDirection == nextDirection &&
                (lastState == NoteBeamType.Start || lastState == NoteBeamType.Continue))
            {
                //Debug.WriteLine("CONTINUE");
                result = NoteBeamType.Continue;
                lastState = result;
                return result;
            }

            if (previousNote != null &&
               currentNote.Length == previousNote.Length &&
               currentDirection == previousDirection &&
               (lastState == NoteBeamType.Start || lastState == NoteBeamType.Continue))
            {
                //Debug.WriteLine("END");
                result = NoteBeamType.End;
                lastState = result;
                return result;
            }

            return result;
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
