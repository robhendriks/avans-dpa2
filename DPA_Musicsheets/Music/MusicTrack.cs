using System.Collections.Generic;
using System.Collections;
using System.Collections.ObjectModel;

namespace DPA_Musicsheets.Music
{
    public class MusicTrack : IEnumerable<MusicNote>
    {
        public string Name { get; set; }
        public ObservableCollection<MusicNote> Notes { get; private set; }

        public MusicTrack()
        {
            Notes = new ObservableCollection<MusicNote>();
        }

        public IEnumerator<MusicNote> GetEnumerator()
        {
            return Notes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Notes.GetEnumerator();
        }

        public override string ToString()
        {
            return $"MusicTrack{{Name={Name}}}";
        }
    }
}
