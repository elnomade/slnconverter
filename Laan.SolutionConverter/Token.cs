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
    public class Token
    {
        // Fields
        public static Token Null = null;

        // Methods
        public Token()
        {
            this.Type = TokenType.None;
            this.Value = "";
        }

        public Token(string value, TokenType type)
        {
            this.Value = value;
            this.Type = type;
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(null, obj))
            {
                return false;
            }
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }
            Token token = obj as Token;
            if (token == Null)
            {
                return false;
            }
            return (this.Value.Equals(token.Value) && this.Type.Equals(token.Type));
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        private bool IsEmpty()
        {
            return (this.Value.Length == 0);
        }

        public bool IsTypeIn(params TokenType[] tokenTypes)
        {
            return tokenTypes.Any<TokenType>(tt => (tt == this.Type));
        }

        public static bool operator ==(Token one, Token two)
        {
            if (object.ReferenceEquals(one, null) && object.ReferenceEquals(two, null))
            {
                return true;
            }
            if (object.ReferenceEquals(one, null) || object.ReferenceEquals(two, null))
            {
                return false;
            }
            return (one.Value.ToLower() == two.Value.ToLower());
        }

        public static bool operator ==(Token one, string two)
        {
            if (object.ReferenceEquals(one, null) && object.ReferenceEquals(two, null))
            {
                return true;
            }
            if (object.ReferenceEquals(one, null) || object.ReferenceEquals(two, null))
            {
                return false;
            }
            return ((one.Value != null) && (one.Value.ToLower() == two.ToLower()));
        }

        public static bool operator !=(Token one, Token two)
        {
            return !(one == two);
        }

        public static bool operator !=(Token one, string two)
        {
            return (one != two);
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", this.Value, this.Type);
        }

        // Properties
        public TokenType Type { get; set; }

        public string Value { get; set; }
    }
}
