using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Laan.SolutionConverter
{
    [DebuggerDisplay("{Type} : {Regex}")]
    public class TokenDefinition
    {
        // Methods
        public TokenDefinition(TokenType type, string regex) : this(type, regex, null)
        {
        }

        public TokenDefinition(TokenType type, string regex, string multiLineTerminator)
        {
            if (!string.IsNullOrEmpty(multiLineTerminator))
            {
                MultiLineTerminator = new Regex(multiLineTerminator, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
                MultiLineContinuation = new Regex(string.Format("[^{0}]*", multiLineTerminator), RegexOptions.Compiled | RegexOptions.ExplicitCapture);
            }
            Regex = new Regex(regex, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
            Type = type;
        }

        // Properties
        public bool IsMultiLine
        {
            get { return (MultiLineTerminator != null); }
        }

        public Regex MultiLineContinuation { get; private set; }

        public Regex MultiLineTerminator { get; private set; }

        public Regex Regex { get; private set; }

        public bool Skip { get; set; }

        public TokenType Type { get; private set; }

        public bool WithinQuotesOnly { get; private set; }
    }
}
