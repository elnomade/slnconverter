using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Laan.SolutionConverter
{
    public enum TokenType
    {
        None,
        WhiteSpace,
        Version,
        Numeric,
        AlphaNumeric,
        Variable,
        OpenBrace,
        CloseBrace,
        OpenBracket,
        CloseBracket,
        SingleQuote,
        DoubleQuote,
        BlockedText,
        Operator,
        Symbol,
        InLineComment,
        MultiLineComment,
        String,
        QuotedText,
        Line,
        Guid,
        NewLine
    }

}
