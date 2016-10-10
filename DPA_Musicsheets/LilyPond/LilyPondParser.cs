using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DPA_Musicsheets.LilyPond
{
    public class LilyPondParser : IDisposable
    {
        private readonly LilyPondLexer lexer;

        private Token[] tokens;
        private int tokenCount;
        private int tokenIndex;

        private string parameterName;

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


        private void Parse()
        {
            Parameters = new Dictionary<string, string>();

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
                        BeginParameter();
                        break;
                    case "NOTE":

                        break;
                    case "PIPE":

                        break;
                }
            }
            tokenIndex--;
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

            Parameters.Add(key, value);
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
