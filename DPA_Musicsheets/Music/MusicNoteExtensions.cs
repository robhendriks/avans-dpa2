using DPA_Musicsheets.Utility;
using PSAMControlLibrary;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DPA_Musicsheets.Music
{
    public static class MusicNoteExtensions
    {
        public enum Mode
        {
            Previous,
            Next
        }

        private static MusicNote GetNote(ICollection<MusicNote> notes, int index = 0, Mode mode = Mode.Previous)
        {
            if (notes.Count < 2) return null;
            return (mode == Mode.Next 
                ? notes.ElementAtOrDefault(index + 1) 
                : notes.ElementAtOrDefault(index - 1));
        }

        public static void Generate(this ICollection<MusicalSymbol> result, List<MusicNote> notes, MusicNote baseNote)
        {
            int i = 0, il = notes.Count;
            foreach (var note in notes)
            {
                if (baseNote == null) baseNote = note;

                if (!note.IsRelative)
                {
                    var previousNote = GetNote(notes, i, Mode.Previous);
                    var nextNote = GetNote(notes, i, Mode.Next);

                    result.Add(MusicalSymbolFactory.Create(
                        baseNote,
                        previousNote,
                        note,
                        nextNote));

                    if (note.HasBarLine)
                    {
                        result.Add(new Barline());
                    }
                }

                i++;
            }
        }
    }
}
