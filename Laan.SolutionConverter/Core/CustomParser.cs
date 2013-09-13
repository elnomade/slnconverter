using System;
using System.Collections.Generic;

namespace Laan.SolutionConverter
{
    public abstract class CustomParser
    {
        public CustomParser(ITokenizer tokenizer)
        {
            Tokenizer = tokenizer;
        }

        protected void ExpectToken(string token)
        {
            if (CurrentToken.ToLower() != token.ToLower())
                throw new ExpectedTokenNotFoundException(token, CurrentToken, Tokenizer.Position);
            
            ReadNextToken();
        }

        protected void ExpectTokens(params string[] tokens)
        {
            foreach (string token in tokens)
                ExpectToken(token);
        }

        protected void ReadNextToken()
        {
            Tokenizer.ReadNextToken();
        }

        protected string CurrentToken
        {
            get { return Tokenizer.Current.Value; }
        }

        protected ITokenizer Tokenizer { get; private set; }
    }
}
