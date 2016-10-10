using DPA_Musicsheets.Utility;

namespace DPA_Musicsheets.Music
{
    public class MusicMeta
    {
        public int BPM { get; set; }
        public TimeSignature TimeSignature { get; set; }

        public override string ToString()
        {
            return $"MusicMeta{{BPM={BPM}, TimeSignature={TimeSignature}}}";
        }
    }
}
