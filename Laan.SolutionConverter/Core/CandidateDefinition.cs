using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Laan.SolutionConverter
{
    [DebuggerDisplay(@"\{ Position = {Position}, Match = {Match}, Definition = {Definition} \}")]
    public class CandidateDefinition
    {
        // Methods
        public CandidateDefinition(int position, Match match, TokenDefinition definition)
        {
            this.Position = position;
            this.Match = match;
            this.Definition = definition;
        }

        public override string ToString()
        {
            return string.Format("{{ Position = {0}, Match = {1}, Definition = {2} }}", this.Position, this.Match, this.Definition);
        }

        // Properties
        public TokenDefinition Definition { get; private set; }

        public Match Match { get; private set; }

        public int Position { get; private set; }
    }
}
