﻿using PSAMControlLibrary;
using System.Collections.Generic;
using System.Diagnostics;

namespace DPA_Musicsheets.Utility
{
    public static class MusicalSymbolFactory
    {
        private static NoteBeamType lastState = NoteBeamType.Single;
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
                //NoteBeamType beamType = NoteBeamType.Single;
                NoteTieType tieType = hasTie(previousNote, note, nextNote);

                Note n = new Note(GetNote(note.Note), 0, note.Octave, GetDuration(note.Length), direction, tieType, new List<NoteBeamType>() { beamType });

                if (note.HasLengthMultiplier)
                {
                    n.NumberOfDots = 1;
                }

                return n;
            }
        }

        private static NoteStemDirection determineDirection(MusicNote baseNote, MusicNote note)
        {
            NoteStemDirection direction = NoteStemDirection.Down;
            if (baseNote != null)
            {
                int baseOctave = baseNote.Octave;
                if (note.Octave <= baseOctave && LilyPond.LilyPondHelper.Compare(note.Note, MusicNoteNote.B) < 0)
                {
                    direction = NoteStemDirection.Up;
                }
            }
            return direction;
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


        public static NoteBeamType isPair(MusicNote previousNote, MusicNote currentNote, MusicNote nextNote, MusicNote baseNote)
        {

            NoteBeamType t = NoteBeamType.Single;
            if (currentNote.Length < 8) return t;
            var previous = determineDirection(baseNote, previousNote);
            var current = determineDirection(baseNote, currentNote);
            var next = determineDirection(baseNote, nextNote);

            if (currentNote.Length == nextNote.Length)
            {
                if (previousNote.Length != currentNote.Length)
                {
                    if (current == next)
                    {
                        if (lastState == NoteBeamType.Single || lastState == NoteBeamType.End)
                        {
                            Debug.WriteLine("Start: " + lastState);
                            t = NoteBeamType.Start;
                            lastState = NoteBeamType.Start;
                        }
                    }
                }
                else
                {
                    if (previous == current && current == next)
                    {
                        if (lastState == NoteBeamType.Start || lastState == NoteBeamType.Continue)
                        {
                            Debug.WriteLine("Continue: " + lastState);
                            t = NoteBeamType.Continue;
                            lastState = NoteBeamType.Continue;
                        }
                    }
                }
            }

            if (currentNote.Length == previousNote.Length && currentNote.Length != nextNote.Length)
            {
                if (previous == current)
                {
                    if (lastState == NoteBeamType.Start || lastState == NoteBeamType.Continue)
                    {
                        Debug.WriteLine("End: " + lastState);
                        t = NoteBeamType.End;
                        lastState = NoteBeamType.End;
                    }
                }
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
