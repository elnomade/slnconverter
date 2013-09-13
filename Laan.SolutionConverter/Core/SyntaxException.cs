using System;
using System.Collections.Generic;
using System.Linq;

namespace Laan.SolutionConverter
{
    [Serializable]
    public class ExpectedTokenNotFoundException : Exception
    {
        internal ExpectedTokenNotFoundException(string token, string foundToken, Position position)
            : base("Expected: '" + token + "' but found: '" + foundToken + "' at " + position.ToString()) { }

        public ExpectedTokenNotFoundException() : base() { }
        public ExpectedTokenNotFoundException(string message) : base(message) { }
        public ExpectedTokenNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }

    [Serializable]
    public class SyntaxException : Exception
    {
        public SyntaxException() : base() { }
        public SyntaxException(string message) : base(message) { }
        public SyntaxException(string message, Exception innerException) : base(message, innerException) { }
    }

    [Serializable]
    public class UnknownTokenException : Exception
    {
        public UnknownTokenException() : base() { }
        public UnknownTokenException(string message) : base("'" + message + "'") { }
        public UnknownTokenException(string message, Exception innerException) : base(message, innerException) { }
    }
}
