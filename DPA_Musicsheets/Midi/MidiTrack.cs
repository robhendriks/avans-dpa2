using System.Collections.ObjectModel;

namespace DPA_Musicsheets
{
    public class MidiTrack
    {
        public string TrackName { get; set; }
        public ObservableCollection<string> Messages { get; private set; }
        public ObservableCollection<MusicNote> Notes { get; private set; }

        public MidiTrack()
        {
            Messages = new ObservableCollection<string>();
            Notes = new ObservableCollection<MusicNote>();
        }
    }
}
