using DPA_Musicsheets.Utility;

namespace DPA_Musicsheets.Music
{
    public class MusicMetaBuilder
    {
        private readonly MusicMeta instance = new MusicMeta();

        public MusicMetaBuilder Signature(TimeSignature timeSignature)
        {
            instance.TimeSignature = timeSignature;
            return this;
        }

        public MusicMetaBuilder BPM(int bpm)
        {
            instance.BPM = bpm;
            return this;
        }

        public MusicMeta Create()
        {
            return instance;
        }
    }
}
