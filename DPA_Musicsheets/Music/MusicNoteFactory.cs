using DPA_Musicsheets.LilyPond;
using DPA_Musicsheets.Midi;

namespace DPA_Musicsheets.Music
{
    public class MusicNoteFactory
    {
        public static MusicNote Create(int length, int keycode, MusicNoteNote noteNote)
        {
            return new MusicNote(length, keycode, noteNote);
        }

        public static MusicNote Create(int length, int keycode)
        {
            return new MusicNote(length, MidiHelper.GetOctave(keycode), MidiHelper.GetNoteNote(keycode));
        }

        public static MusicNote Create(Token token)
        {
            int i = 5;
            char[] a = token.Value.ToCharArray();
            if (a.Length > 1)
            {
                int j = 0;
                while ( j < a.Length - 1)
                {
                    i++;
                    j++;
                }
            }
            System.Diagnostics.Debug.WriteLine("IK BEN OKTAAF DE BOLLE: " + i);

            //-1 because this note (the \relative note) doesn't have a length.
            return new MusicNote(-1, i, MusicNoteNote.C);

        }
    }
}
