using DPA_Musicsheets.Utility;

namespace DPA_Musicsheets.Music
{
    public class MusicTrack
    {
        public string Name { get; set; }

        public MusicTrack()
        {
        }

        public override string ToString()
        {
            return $"MusicTrack{{Name={Name}}}";
        }
    }
}
