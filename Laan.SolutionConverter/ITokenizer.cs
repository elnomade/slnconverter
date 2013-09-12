using System;
using System.Collections.Generic;
using System.Linq;

namespace Laan.SolutionConverter
{
    public interface ITokenizer
    {
        // Methods
        BracketStructure ExpectBrackets();
        void ExpectToken(string token);
        void ExpectTokens(string[] tokens);
        bool IsNextToken(params string[] tokenSet);
        void ReadNextToken();
        bool TokenEquals(string value);

        // Properties
        Token Current { get; }
        bool HasMoreTokens { get; }
        Position Position { get; }
    }
}
