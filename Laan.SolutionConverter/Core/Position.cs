using System;
using System.Collections.Generic;
using System.Linq;

namespace Laan.SolutionConverter
{
    public class Position
    {
        private CustomTokenizer _tokenizer;

        public Position(CustomTokenizer tokenizer)
        {
            _tokenizer = tokenizer;
            Row = 0;
            Column = 1;
        }

        internal void NewRow()
        {
            Row++;
            Column = 1;
        }

        public override string ToString()
        {
            int length = 0;
            if ((_tokenizer != null) && (_tokenizer.Current != Token.Null))
            {
                length = _tokenizer.Current.Value.Length;
            }
            return string.Format("Row: {0}, Col: {1}", Row, Column - length);
        }

        public int Column { get; set; }
        public int Row { get; set; }
    }
}
