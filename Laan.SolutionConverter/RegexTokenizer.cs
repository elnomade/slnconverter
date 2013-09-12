using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Laan.SolutionConverter
{
    [DebuggerDisplay("Current: {Current} Position: {Position} [{HasMoreTokens ? \"Y\" : \"N\"}]")]
    public abstract class RegexTokenizer : CustomTokenizer, IDisposable
    {
        private readonly TextReader _reader;

        private Dictionary<TokenType, bool> _skippedTokens;
        private Token _current;
        private string _line;

        protected RegexTokenizer(string input)
        {
            TokenDefinitions = new List<TokenDefinition>();

            Position = new Position(this);
            _reader = new StringReader(input);
            _line = String.Empty;
        }

        private void AdvanceCurrentToken(Match match, TokenType type)
        {
            _current = new Token(match.Value, type);
            _line = _line.Remove(match.Captures[0].Index, match.Captures[0].Length);
            Position.Column += match.Value.Length;
        }

        private CandidateDefinition GetCandidateDefinition(IList<CandidateDefinition> definitions)
        {
            var candidateDefinitions = definitions
                .Where(m => m.Match.Success)
                .OrderBy(m => m.Match.Captures[0].Index)
                .ThenBy(td => td.Position)
                .ToList();

            var matchingToken = candidateDefinitions.FirstOrDefault();

            if (matchingToken != null)
                AdvanceCurrentToken(matchingToken.Match, matchingToken.Definition.Type);

            return matchingToken;
        }

        private void ProcessMultiLine(CandidateDefinition matchingToken)
        {
            var multiLineType = _current.Type;
            var multiLineToken = new System.Text.StringBuilder(_current.Value);

            Match continuation;
            do
            {
                continuation = matchingToken.Definition.MultiLineContinuation.Match(_line);
                if (continuation.Value.Length > 0)
                {
                    AdvanceCurrentToken(
                        continuation,
                        matchingToken.Definition.Type
                    );

                    var linesConsumed = ReadNextLine();
                    multiLineToken.Append(_current.Value);
                    for (int i = 0; i < linesConsumed; i++)
                        multiLineToken.AppendLine();
                }
            }
            while (HasMoreTokens && continuation != null && continuation.Value.Length > 0);

            Match terminationMatch = matchingToken.Definition.MultiLineTerminator.Match(_line);
            if (terminationMatch.Value.Length < 0)
                throw new SyntaxException(String.Format("Failed to find terminal for {0}", matchingToken.Definition.Type));

            AdvanceCurrentToken(terminationMatch, matchingToken.Definition.Type);
            multiLineToken.Append(_current.Value);

            _current = new Token(multiLineToken.ToString(), multiLineType);
        }

        private int ReadNextLine()
        {
            int linesConsumed = 0;
            
            while (_line.Length == 0 && _reader.Peek() != -1)
            {
                if (Position.Column > 1)
                    _current = new Token { Type = TokenType.NewLine };

                _line = _reader.ReadLine();
                Position.NewRow();
                linesConsumed++;
            }

            return linesConsumed;
        }

        protected abstract void InitialiseTokenDefinitions();
        protected List<TokenDefinition> TokenDefinitions { get; set; }

        public virtual void Initialise()
        {
            InitialiseTokenDefinitions();
            _skippedTokens = TokenDefinitions.ToDictionary(td => td.Type, td => false);
            ReadNextToken();
        }
        
        public override void ReadNextToken()
        {
            _current = null;
            ReadNextLine();

            if (_current != Token.Null && _current.Type == TokenType.NewLine)
                return;

            if (!HasMoreTokens)
                return;

            var m = TokenDefinitions
                .Select((td, i) => new CandidateDefinition(i, td.Regex.Match(_line), td))
                .ToList();

            var matchingToken = GetCandidateDefinition(m);
            if (matchingToken == null)
                throw new SyntaxException();

            if (matchingToken.Definition.IsMultiLine)
                ProcessMultiLine(matchingToken);

            if (Current == Token.Null)
                return;

            bool skippable;
            if ((_skippedTokens.TryGetValue(Current.Type, out skippable)) && skippable)
                ReadNextToken();
        }

        public void SetSkip(bool skipped, params TokenType[] tokenTypes)
        {
            foreach (var tokenType in tokenTypes)
                _skippedTokens[tokenType] = skipped;
        }

        public override Token Current
        {
            get { return _current; }
        }

        public override bool HasMoreTokens
        {
            get
            {
                return (_line != null && _line.Length > 0)
                    || _reader.Peek() != -1
                    || Current != Token.Null;
            }
        }

        public void Dispose()
        {
            _reader.Dispose();
        }
    }
}
