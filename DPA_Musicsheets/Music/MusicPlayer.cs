using Sanford.Multimedia.Midi;
using System;
using System.Diagnostics;

namespace DPA_Musicsheets.Utility
{
    public class MusicPlayer
    {
        private readonly OutputDevice outputDevice = new OutputDevice(0);

        private MidiPlayer player;

        public void Play(string fileName)
        {
            Stop();

            player = new MidiPlayer(outputDevice);
            player.Play(fileName);
        }

        public void Stop()
        {
            if (player != null)
            {
                player.Dispose();
            }
        }
    }
}
