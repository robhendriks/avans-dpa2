using DPA_Musicsheets.Music;
using PSAMControlLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DPA_Musicsheets.LilyPond
{
    public class LilyPondParser : IDisposable
    {
        private readonly LilyPondLexer lexer;

        private Token[] tokens;
        private int tokenCount;
        private int tokenIndex;

        private string parameterName;

        public MusicNote RelativeNote { get; private set; } // Starts at Octave 5!!!
        public List<MusicNote> Notes { get; private set; }

        public Dictionary<string, string> Parameters { get; private set; }

        public LilyPondParser(LilyPondLexer lexer)
        {
            if (lexer == null)
            {
                throw new ArgumentNullException(nameof(lexer));
            }
            this.lexer = lexer;
            Parse();
        }

        public static List<MusicalSymbol> parse(TextReader reader, out string source)
        {
            var result = new List<MusicalSymbol>();

            var lexer = new LilyPondLexer(reader);
            var parser = new LilyPondParser(lexer);

            foreach (var pair in parser.Parameters)
            {
                switch (pair.Key)
                {
                    case "clef":
                        if (pair.Value == "treble")
                        {
                            result.Add(new Clef(ClefType.GClef, 2));
                        }
                        else if (pair.Value == "bass")
                        {
                            result.Add(new Clef(ClefType.FClef, 2));
                        }
                        else
                        {
                            result.Add(new Clef(ClefType.CClef, 2));
                        }
                        break;
                    case "time":
                        var parts = pair.Value.Split('/');

                        uint beats = uint.Parse(parts[0]);
                        uint beatType = uint.Parse(parts[1]);

                        result.Add(new TimeSignature(TimeSignatureType.Numbers, beats, beatType));
                        break;
                    case "tempo":
                        break;
                }
            }

            source = lexer.Source;
            var baseNote = (parser.Notes.Count > 2 ? parser.Notes[1] : null);
            result.Generate(parser.Notes, baseNote);
            return result;
        }

        private void Parse()
        {
            //TODO: Check if input is from Lexer or Editor.

            Parameters = new Dictionary<string, string>();
            Notes = new List<MusicNote>();

            tokens = lexer.Tokens.ToArray();
            tokenCount = tokens.Length;
            tokenIndex = 0;

            BeginDocument();
        }

        private Token Next()
        {
            return (tokenIndex < tokenCount ? tokens[tokenIndex++] : null);
        }

        private void BeginDocument()
        {
            Token token;
            if ((token = Next()) == null)
            {
                throw new LilyPondException("Expected FUNCTION but was EOF");
            }
            if (token.Type != "FUNCTION")
            {
                throw new LilyPondException($"Expected FUNCTION but was {token.Type}");
            }

            BeginFunction();
        }

        private void BeginFunction()
        {
            Token token;

            // Peek at NOTE
            if ((token = Next()) == null)
            {
                throw new LilyPondException("Expected NOTE but was EOF");
            }
            if (token.Type != "NOTE")
            {
                throw new LilyPondException($"Expected NOTE but was {token.Type}");
            }

            var relNote = MusicNoteFactory.Create(token);
            relNote.IsRelative = true;
            Notes.Add(relNote);

            // Peek at Curly Brace
            if ((token = Next()) == null)
            {
                throw new LilyPondException("Expected CURLY_OPEN but was EOF");
            }
            if (token.Type != "CURLY_OPEN")
            {
                throw new LilyPondException($"Expected CURLY_OPEN but was {token.Type}");
            }

            NextNonCurlyBrace();
            EndFunction();
        }

        private void NextNonCurlyBrace()
        {
            Token token;
            while ((token = Next()) != null && token.Type != "CURLY_CLOSE")
            {
                switch (token.Type)
                {
                    case "FUNCTION":
                        parameterName = token.Value;

                        if (parameterName == "\\repeat" || parameterName == "\\alternative")
                        {
                            SkipUntilCurlyBrace();
                        }
                        else if (parameterName == "\\time" || parameterName == "\\clef" || parameterName == "\\tempo")
                        {
                            BeginParameter();
                        }
                        else
                        {
                            throw new LilyPondException($"Unexpected parameter {parameterName}");
                        }
                        break;
                    case "NOTE":
                        AddMusicNote(token.Value);
                        break;
                    case "PIPE":
                        AddBarLine();
                        break;
                    case "TILDE":
                        AddTie();
                        break;
                    case "CURLY_OPEN":
                        SkipUntilCurlyBrace();
                        break;
                }
            }
            tokenIndex--;
        }

        private void SkipUntilCurlyBrace()
        {
            Token token;
            while ((token = Next()) != null)
            {
                if (token.Type == "CURLY_CLOSE")
                {
                    //tokenIndex--;
                    break;
                }
                tokenIndex++;
            }
        }

        private void AddMusicNote(string str)
        {


            //TODO: Check which note & octave the current note is based on previous Note.

            Regex regex = new Regex("([a-z])(is|es)?('|,)?([0-9]+)", RegexOptions.IgnoreCase);
            var match = regex.Match(str);
            if (match.Success)
            {
                var note = match.Groups[1].ToString();
                var noteModifier = match.Groups[2].ToString();
                var octaveModifier = match.Groups[3].ToString();
                var noteLength = int.Parse(match.Groups[4].ToString());

                var noteNote = LilyPondHelper.Create(note, noteModifier);

                var previousNote = GetPrevious();

                var octave = LilyPondHelper.Octave(noteNote, octaveModifier, previousNote);
                var nextNote = new MusicNote(noteLength, octave, noteNote);

                if (str.Contains("."))
                {
                    nextNote.HasLengthMultiplier = true;
                }

                Notes.Add(nextNote);

            }
        }

        private MusicNote GetPrevious()
        {
            if (Notes.Count == 0)
            {
                return null;
            }

            int i = Notes.Count - 1;
            MusicNote note = null;
            while ((note = Notes[i]).Note == MusicNoteNote.Rest)
            {
                i--;
                if (i < 0) break;
            }
            return note;
        }

        private void AddBarLine()
        {
            Notes.ElementAt(Notes.Count - 1).HasBarLine = true;
        }

        private void AddTie()
        {
            Notes.ElementAt(Notes.Count - 1).HasTie = true;
        }

        private void BeginParameter()
        {
            Token token;

            // Peek at Time Signature, Tempo or Clef
            if ((token = Next()) == null)
            {
                throw new LilyPondException("Expected TIME_SIGNATURE, TEMPO or CLEF but was EOF");
            }
            if (token.Type != "TIME_SIGNATURE" && token.Type != "TEMPO" && token.Type != "CLEF")
            {
                throw new LilyPondException($"Expected TIME_SIGNATURE, TEMPO or CLEF but was {token.Type}");
            }

            string key = parameterName.TrimStart(new char[] {'\\'});
            string value = token.Value.Trim();

            //TODO: Check for double keys. (multiple time commands?)

            if (!Parameters.ContainsKey(key))
            {
                Parameters.Add(key, value);
            }
        }

        private void EndFunction()
        {
            Token token;
            if ((token = Next()) == null)
            {
                throw new LilyPondException("Expected CURLY_CLOSE but was EOF");
            }
            if (token.Type != "CURLY_CLOSE")
            {
                throw new LilyPondException($"Expected CURLY_CLOSE but was {token.Type}");
            }
        }

        public void Dispose()
        {
            lexer.Dispose();
        }
    }
}
