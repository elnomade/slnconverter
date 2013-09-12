using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Laan.SolutionConverter
{
    public class Program
    {
        private static void VerifyPath(string path)
        {
            if (String.IsNullOrEmpty(path))
                throw new ArgumentException("file not supplied as an argument");

            if (!File.Exists(path))
                throw new ArgumentException("file not found");

            if (Path.GetExtension(path) != ".sln")
                throw new ArgumentException("file must be a VS solution file (.sln)");
        }

        private static void Main(string[] args)
        {
            try
            {
                string path = args.FirstOrDefault();
                VerifyPath(path);

                var tokenizer = new SlnTokenizer(File.ReadAllText(path));
                tokenizer.Initialise();
                tokenizer.SetSkip(true, TokenType.WhiteSpace);

                while (tokenizer.HasMoreTokens)
                {
                    Token current = tokenizer.Current;
                    string value = current.Type == TokenType.NewLine ? "CRLF" : current.Value;

                    Console.WriteLine(String.Format("{0,-20}| {1}", current.Type, value));
                    tokenizer.ReadNextToken();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
            Console.Read();
        }
    }
}
