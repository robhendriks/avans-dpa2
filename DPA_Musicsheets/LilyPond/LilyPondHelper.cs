using System;
using System.Diagnostics;

namespace DPA_Musicsheets.LilyPond
{
    public static class LilyPondHelper
    {
        public static readonly MusicNoteNote[] Notes =
        {
            MusicNoteNote.C, MusicNoteNote.CSharp,
            MusicNoteNote.D, MusicNoteNote.DSharp,
            MusicNoteNote.E,
            MusicNoteNote.F, MusicNoteNote.FSharp,
            MusicNoteNote.G, MusicNoteNote.GSharp,
            MusicNoteNote.A, MusicNoteNote.ASharp,
            MusicNoteNote.B
        };

        public static MusicNoteNote Create(string note, string modifier)
        {
            var str = note.ToUpper();
            if (str == "R")
            {
                return MusicNoteNote.Rest;
            }
            if (modifier == "is")
            {
                str += "Sharp";
            }
            MusicNoteNote result;
            return (Enum.TryParse(str, out result) ? result : MusicNoteNote.None);
        }

        public static int Find(MusicNoteNote noteNote)
        {
            for (var i = 0; i < Notes.Length; i++)
            {
                if (Notes[i] == noteNote)
                {
                    return i;
                }
            }
            return -1;
        }

        public static int Compare(MusicNoteNote a, MusicNoteNote b)
        {
            int aIndex = Find(a);
            int bIndex = Find(b);
            if (aIndex == bIndex) return 0;
            return (aIndex > bIndex ? 1 : -1);
        }

        public static int FindNearest(MusicNoteNote sourceNoteNote, MusicNoteNote targetNoteNote)
        {
            int baseIndex = Find(sourceNoteNote);
            if (baseIndex == -1)
            {
                return -1;
            }

            MusicNoteNote tmpNoteNote;

            // Iterate left
            int left = baseIndex - 1;
            int leftSteps = 0;
            int leftCycles = 0;

            if (left < 0)
            {
                left = Notes.Length - 1;
                leftCycles++;
            }

            while ((tmpNoteNote = Notes[left]) != targetNoteNote)
            {
                left--;
                leftSteps++;
                if (left < 0)
                {
                    leftCycles++;
                    left = Notes.Length - 1;
                }
                if (leftSteps == Notes.Length) break;
            }

           //WriteLine($"[LEFT] FOUND {targetNoteNote} RELATIVE TO {sourceNoteNote} AFTER {leftSteps} STEP(S) AND {leftCycles} CYCLE(S)");

            // Iterate right
            int right = baseIndex + 1;
            int rightSteps = 0;
            int rightCycles = 0;

            if (right == Notes.Length)
            {
                right = 0;
                rightCycles++;
            }

            while ((tmpNoteNote = Notes[right]) != targetNoteNote)
            {
                right++;
                rightSteps++;
                if (right == Notes.Length)
                {
                    rightCycles++;
                    right = 0;
                }
                if (rightSteps == Notes.Length) break;
            }

            //Debug.WriteLine($"[RIGHT] FOUND {targetNoteNote} RELATIVE TO {sourceNoteNote} AFTER {rightSteps} STEP(S) AND {rightCycles} CYCLE(S)");

            return (leftSteps > rightSteps ? rightCycles : -leftCycles);
        }

        public static int Octave(MusicNoteNote noteNote, string modifier, MusicNote previousNote)
        {
            int diff = (previousNote.Note != noteNote ? FindNearest(previousNote.Note, noteNote) : 0);
            int octave = previousNote.Octave;
            if (diff > 0)
            {
                octave += diff;
            }
            else
            {
                octave -= Math.Abs(diff);
            }

            if (modifier == "'")
            {
                octave += 1;
            }
            else if (modifier == ",")
            {
                octave -= 1;
            }

            //Debug.WriteLine($"[OCTAVE]\t{noteNote}\t{octave}");

            return octave;
        }
    }
}
