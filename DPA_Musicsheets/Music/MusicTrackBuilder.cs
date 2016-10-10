using DPA_Musicsheets.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Music
{
    public class MusicTrackBuilder
    {
        private readonly MusicTrack instance = new MusicTrack();

        public MusicTrackBuilder Name(string name)
        {
            instance.Name = name ?? "Unknown";
            return this;
        }

        public MusicTrackBuilder Note()
        {
            // TODO
            return this;
        }

        public MusicTrack Create()
        {
            return instance;
        }
    }
}
