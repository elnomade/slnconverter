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

        private static string ConvertInput(string path)
        {
            return String.Format(".\\{0}.xml", Path.GetFileNameWithoutExtension(path));
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

                SolutionParser solutionParser = new SolutionParser(tokenizer);

                var document = solutionParser.Execute();

                var converter = new Converter();
                string outputName = args.Skip(1).FirstOrDefault() ?? ConvertInput(path);
                converter.WriteDocument(document, outputName);

                Console.WriteLine("Done...");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
            Console.Read();
        }
    }
}
