using DPA_Musicsheets.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace DPA_Musicsheets.LilyPond
{
    public class TokenDefinition
    {
        public string Type { get; set; }
        public Regex Regex { get; set; }
        public bool IsIgnored { get; set; }

        public TokenDefinition(string type, string pattern, bool isIgnored = false)
        {
            Type = type;
            Regex = new Regex(pattern);
            IsIgnored = isIgnored;
        }
    }

    public class TokenPosition
    {
        public int Index { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }

        public TokenPosition(int index, int line, int column)
        {
            Index = index;
            Line = line;
            Column = column;
        }
    }

    public class Token
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public TokenPosition Position { get; set; }

        public Token(string type, string value, TokenPosition position)
        {
            Type = type;
            Value = value;
            Position = position;
        }
    }
    public class LilyPondLexer : IDisposable
    {
        public static readonly TokenDefinition[] Rules =
        {
            new TokenDefinition("WHITESPACE", @"\s+", true),
            new TokenDefinition("FUNCTION", @"\\[\w]+"),
            new TokenDefinition("CURLY_OPEN", @"\{"),
            new TokenDefinition("CURLY_CLOSE", @"\}"),
            new TokenDefinition("TIME_SIGNATURE", @"([\d]+)\/([\d]+)"),
            new TokenDefinition("TEMPO", @"([\d]+)\=([\d]+)"),
            new TokenDefinition("CLEF", @"(treble|bass|alto)"),
            new TokenDefinition("PIPE", @"\|"),
            //new TokenDefinition("REST_NOTE", @"r([\d]+)"),
            new TokenDefinition("NOTE", @"[\w\d\.\,\']+"),
            new TokenDefinition("TILDE", @"\~")
        };

        public static readonly Regex EOL = new Regex("\n");

        public IEnumerable<Token> Tokens { get; private set; }

        private readonly TextReader reader;

        public string Source { get; private set; }

        public LilyPondLexer(TextReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }
            this.reader = reader;
            Tokens = Tokenize();
        }

        private IEnumerable<Token> Tokenize()
        {
            Source = ReadLines().ConcatLines();

            int currentIndex = 0;
            int currentLine = 1;
            int currentColumn = 0;

            while (currentIndex < Source.Length)
            {
                TokenDefinition matchedDefinition = null;
                int matchLength = 0;

                foreach (var rule in Rules)
                {
                    var match = rule.Regex.Match(Source, currentIndex);

                    if (match.Success && (match.Index - currentIndex) == 0)
                    {
                        matchedDefinition = rule;
                        matchLength = match.Length;
                        break;
                    }
                }

                if (matchedDefinition == null)
                {
                    throw new LilyPondException($"Unrecognized symbol '{Source[currentIndex]}' at index {currentIndex} (line {currentLine}, column {currentColumn}).");
                }
                else
                {
                    var value = Source.Substring(currentIndex, matchLength);

                    if (!matchedDefinition.IsIgnored)
                    {
                        yield return new Token(matchedDefinition.Type, value, new TokenPosition(currentIndex, currentLine, currentColumn));
                    }

                    var endOfLineMatch = EOL.Match(value);
                    if (endOfLineMatch.Success)
                    {
                        currentLine += 1;
                        currentColumn = value.Length - (endOfLineMatch.Index + endOfLineMatch.Length);
                    }
                    else
                    {
                        currentColumn += matchLength;
                    }

                    currentIndex += matchLength;
                }
            }

            yield return new Token("EOF", null, new TokenPosition(currentIndex, currentLine, currentColumn));
        }

        public IEnumerable<string> ReadLines()
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                yield return line;
            }
        }

        public void Dispose()
        {
            reader.Dispose();
        }
    }
}
