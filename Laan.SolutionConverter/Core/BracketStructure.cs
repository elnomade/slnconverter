using System;
using System.Collections.Generic;
using System.Linq;

namespace Laan.SolutionConverter
{
    public class BracketStructure : IDisposable
    {
        private ITokenizer _tokenizer;

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
