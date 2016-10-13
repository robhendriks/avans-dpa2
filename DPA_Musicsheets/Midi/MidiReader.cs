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
        private List<MusicNote> _notes = new List<MusicNote>();

        public List<MusicNote> Notes
        {
            get { return _notes; }
            private set { _notes = value; }
        }

        private readonly Sequence sequence = new Sequence();
        private readonly MusicMetaBuilder metaBuilder = new MusicMetaBuilder();

        private MusicTrackBuilder trackBuilder;

        private MusicMeta _meta;

        public MusicMeta Meta
        {
            get { return _meta; }
            private set { _meta = value; }
        }

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
                    var eventBuffer = new MidiEvent[3];
                    for (int j = 0; j < 3; j++)
                    {
                        eventBuffer[j] = midiEvents.ElementAtOrDefault(i + j);
                    }

                    var messageBuffer = new ChannelMessage[3];
                    for (int j = 0; j < 3; j++)
                    {
                        messageBuffer[j] = eventBuffer[j]?.MidiMessage as ChannelMessage;
                    }

                    // eventBuffer[0] current
                    // eventBuffer[1] next
                    // eventBuffer[2] the next after next

                    switch (eventBuffer[0].MidiMessage.MessageType)
                    {
                        case MessageType.Channel:
                            if (messageBuffer[0].Command == (ChannelCommand.NoteOn | ChannelCommand.NoteOff))
                            {
                                ParseChannelMessage(eventBuffer, messageBuffer);
                                i += 2;
                                continue;
                            }

                            break;
                        case MessageType.Meta:
                            ParseMeta(eventBuffer[0].MidiMessage as MetaMessage);
                            break;
                    }

                    i++;
                }

                if (Meta == null)
                {
                    Meta = metaBuilder.Create();
                    Debug.WriteLine(Meta);
                }
                else
                {
                    var newTrack = trackBuilder.Create();
                    Notes.AddRange(newTrack.Notes);
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

        private void ParseChannelMessage(MidiEvent[] eventBuffer, ChannelMessage[] messageBuffer)
        {
            if (eventBuffer[0] == null || messageBuffer[0] == null || eventBuffer[1] == null)
            {
                return;
            }

            bool isRest = false;
            if (messageBuffer[1] != null && messageBuffer[2] != null)
            {
                isRest = (eventBuffer[2].AbsoluteTicks - eventBuffer[1].AbsoluteTicks > 0);
            }

            double deltaTicks = eventBuffer[1].AbsoluteTicks - eventBuffer[0].AbsoluteTicks;
            double percentageOfBeatNote = deltaTicks / sequence.Division;
            double percentageOfWholeNote = (1.0 / _meta.TimeSignature.B) * percentageOfBeatNote;

            for (int noteLength = 32; noteLength >= 1; noteLength /= 2)
            {
                double absoluteNoteLength = (1.0 / noteLength);

                if (percentageOfWholeNote <= absoluteNoteLength) {
                    trackBuilder.Note(isRest
                        ? MusicNoteFactory.Create(noteLength, messageBuffer[0].Data1, MusicNoteNote.Rest)
                        : MusicNoteFactory.Create(noteLength, messageBuffer[0].Data1));
                    break;
                }
            }
        }
    }
}
