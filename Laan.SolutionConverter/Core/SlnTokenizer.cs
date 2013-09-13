using System;
using System.Collections.Generic;
using System.Linq;

namespace Laan.SolutionConverter
{
    public class SlnTokenizer : RegexTokenizer, ITokenizer
    {
        public SlnTokenizer(string input): base(input)
        {
        }

        protected override void InitialiseTokenDefinitions()
        {
            base.TokenDefinitions.AddRange(new[] 
            { 
                new TokenDefinition(TokenType.InLineComment, @"\#.*$"), 
                new TokenDefinition(TokenType.String, "'", "'"), 
                new TokenDefinition(TokenType.Guid, @"\{[A-F0-9]{8}(?:-[A-F0-9]{4}){3}-[A-F0-9]{12}\}"), 
                new TokenDefinition(TokenType.AlphaNumeric, @"[A-Za-z_\|]+(\w|\| )*"), 
                new TokenDefinition(TokenType.Version, @"[0-9\.]+"), 
                new TokenDefinition(TokenType.Numeric, @"-?[0-9,]+(\.\d+)"),
                new TokenDefinition(TokenType.OpenBracket, @"\("), 
                new TokenDefinition(TokenType.CloseBracket, @"\)"), 
                new TokenDefinition(TokenType.BlockedText, "\".*?\""), 
                new TokenDefinition(TokenType.Symbol, @"[\=,\.\|]"), 
                new TokenDefinition(TokenType.WhiteSpace, @"[ |\t]+"), 
                new TokenDefinition(TokenType.Line, "^.*$") 
            });
        }
    }
}