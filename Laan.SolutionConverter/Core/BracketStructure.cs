using System;
using System.Collections.Generic;
using System.Linq;

namespace Laan.SolutionConverter
{
    public class BracketStructure : IDisposable
    {
        // Fields
        private ITokenizer _tokenizer;

        // Methods
        public BracketStructure(ITokenizer tokenizer)
        {
            this._tokenizer = tokenizer;
            this._tokenizer.ExpectToken("(");
        }

        public void Dispose()
        {
            this._tokenizer.ExpectToken(")");
        }
    }
}
