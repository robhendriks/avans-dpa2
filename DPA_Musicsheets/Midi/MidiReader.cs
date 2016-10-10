using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DPA_Musicsheets.Midi;
using DPA_Musicsheets.Music;
using System.Diagnostics;

namespace DPA_Musicsheets
{
    public class MidiReader
    {
        private readonly Sequence sequence = new Sequence();
        private readonly MusicMetaBuilder metaBuilder = new MusicMetaBuilder();

        private MusicTrackBuilder trackBuilder;

        private MusicMeta meta;

        public MidiReader(string filename)
        {
            Load(filename);
        }

        private void Load(string filename)
        {
            try
            {
                sequence.Load(filename);
                ReadSequence();
            }
            catch (IOException e)
            {
                throw new MidiLoadException("Unable to load MIDI", e);
            }
        }

        private void ReadSequence()
        {
            foreach(var track in sequence)
            {
                trackBuilder = new MusicTrackBuilder();
                var midiEvents = track.Iterator();
                var count = midiEvents.Count();

                int i = 0;
                while (i < count)
                {
                    var midiEvent = midiEvents.ElementAtOrDefault(i);
                    var nextMidiEvent = midiEvents.ElementAtOrDefault(i + 1);

                    switch (midiEvent.MidiMessage.MessageType)
                    {
                        case MessageType.Channel:
                            var msg = midiEvent.MidiMessage as ChannelMessage;
                            if (msg.Command == (ChannelCommand.NoteOn | ChannelCommand.NoteOff))
                            {
                                //Debug.WriteLine($"@{i}\t<{msg.Command}>\t{msg.Data1}\t{msg.Data2}\t{midiEvent.AbsoluteTicks}\t@->{i + 1}");

                                ParseChannelMessage(midiEvent?.MidiMessage as ChannelMessage, midiEvent, nextMidiEvent);

                                i += 2;
                                continue;
                            }

                            break;
                        case MessageType.Meta:
                            ParseMeta(midiEvent.MidiMessage as MetaMessage);
                            break;
                    }

                    i++;
                }

                Debug.WriteLine("IK BEN KLAAR. DOEI");

                if (meta == null)
                {
                    meta = metaBuilder.Create();
                    Debug.WriteLine(meta);
                }
                else
                {
                    var newTrack = trackBuilder.Create();
                    foreach (var note in newTrack.Notes)
                    {
                        Debug.WriteLine(note);
                    }
                }
            }
        }

        private void ParseMeta(MetaMessage message)
        {
            byte[] b = message.GetBytes();

            switch (message.MetaType)
            {
                case MetaType.Tempo:
                    // Set meta BPM (tempo)
                    int tempo = (b[0] & 0xff) << 16 | (b[1] & 0xff) << 8 | (b[2] & 0xff);
                    int bpm = 60000000 / tempo;
                    metaBuilder.BPM(bpm);
                    break;
                case MetaType.TimeSignature:
                    // Set meta time signature (maatsoort)
                    int beforeSlash = b[0];
                    int afterSlash = (int)Math.Pow(2, b[1]);
                    metaBuilder.Signature(new Utility.TimeSignature(beforeSlash, afterSlash));
                    break;
                case MetaType.TrackName:
                    // Set track name
                    trackBuilder.Name(Encoding.Default.GetString(message.GetBytes()));
                    break;
                default:
                    break;
            }
        }

        private void ParseChannelMessage(ChannelMessage message, MidiEvent current, MidiEvent next)
        {
            if (message == null || current == null || next == null)
            {
                return;
            }

            double deltaTicks = next.AbsoluteTicks - current.AbsoluteTicks;
            double percentageOfBeatNote = deltaTicks / sequence.Division;
            double percentageOfWholeNote = (1.0 / meta.TimeSignature.B) * percentageOfBeatNote;

            for (int noteLength = 32; noteLength >= 1; noteLength /= 2)
            {
                double absoluteNoteLength = (1.0 / noteLength);

                if (percentageOfWholeNote <= absoluteNoteLength)
                {
                    trackBuilder.Note(new MusicNote(noteLength, message.Data1));
                    break;
                }
            }
        }
    }
}
