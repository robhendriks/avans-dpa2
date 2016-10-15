using DPA_Musicsheets.Utility;
using PSAMControlLibrary;
using System.Collections.Generic;

namespace DPA_Musicsheets.Music
{
    public static class MusicNoteExtensions
    {
        public static void Generate(this ICollection<MusicalSymbol> result, List<MusicNote> notes, MusicNote baseNote)
        {
            int i = 0, il = notes.Count;
            foreach (var note in notes)
            {

                if (baseNote == null) baseNote = note;
                if (i == notes.Count - 1) i = notes.Count - 2;

                if (!note.IsRelative)
                {


                    result.Add(MusicalSymbolFactory.Create(baseNote, note,
                        (i < il ? notes[i + 1] : null),
                        (i > 0 ? notes[i - 1] : null)));
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
