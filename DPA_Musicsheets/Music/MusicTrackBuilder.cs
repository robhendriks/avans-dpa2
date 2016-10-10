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
            if (name != null)
            {
                instance.Name = name;
            }
            return this;
        }

        public MusicTrackBuilder Note(MusicNote note)
        {
            if (note != null)
            {
                instance.Notes.Add(note);
            }
            return this;
        }

        public MusicTrack Create()
        {
            return instance;
        }
    }
}
