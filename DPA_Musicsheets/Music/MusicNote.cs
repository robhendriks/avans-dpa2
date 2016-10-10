﻿using System;
using System.Diagnostics;
using Sanford.Multimedia.Midi;
using System.Linq;
using System.Collections.Generic;

namespace DPA_Musicsheets
{
    /**
     * Als Data2 == 90, NOTE_ON
     * Als Data2 == 0, NOTE_OFF
     * TODO: Check fo
     */
    public class MusicNote
    {
        public static readonly IEnumerable<MusicNoteNote> Notes = Enum.GetValues(typeof(MusicNoteNote)).Cast<MusicNoteNote>();

        public double Length { get; private set; }
        public int Octave { get; private set; }
        public MusicNoteNote Note { get; private set; }

        public MusicNote(int length, int keycode)
        {
            Length = length;
            Octave = GetOctave(keycode);
            Note = GetNote(keycode);
        }

        public static int GetLength()
        {
            return -1;
        }

        public static int GetOctave(int keycode)
        {
            return (keycode / 12) - 1;
        }

        public static MusicNoteNote GetNote(int keycode)
        {
            return Notes.ElementAt(keycode % 12);
        }

        public override string ToString()
        {
            return "MusicNote{Length=" + Length
                + ", Octave=" + Octave
                + ", Note=" + Note
                + "}";
        }
    }
}
