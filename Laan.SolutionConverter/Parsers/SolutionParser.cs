using System;
using System.Collections.Generic;
using System.Linq;

namespace Laan.SolutionConverter
{
    public class SolutionParser : CustomParser
    {
        private readonly SlnTokenizer _tokenizer;

        public SolutionParser(SlnTokenizer tokenizer) : base(tokenizer)
        {
            _tokenizer = tokenizer;
        }

        private string ReadUntil(TokenType tokenType, string joinSeparator = "")
        {
            var headerItems = new List<string>();
            while (Tokenizer.Current.Type != tokenType)
            {
                headerItems.Add(Tokenizer.Current.Value);
                ReadNextToken();
            }

            return String.Join(joinSeparator, headerItems);
        }

        private string ReadString()
        {
            var token = Tokenizer.Current.Value.Trim('"');
            ReadNextToken();
            return token;
        }

        private void ReadNameValuePair(Dictionary<string, string> info)
        {
            _tokenizer.SetSkip(false, TokenType.WhiteSpace);
            var name = ReadUntil(TokenType.Symbol, "").Trim();
            ExpectToken("=");

            var value = ReadUntil(TokenType.NewLine, "").Trim();

            _tokenizer.SetSkip(true, TokenType.WhiteSpace);
            ReadNextToken();

            info[name] = value;
        }

        private void ParseHeader(SolutionDocument document)
        {
            _tokenizer.SetSkip(false, TokenType.WhiteSpace);

            string header = ReadUntil(TokenType.Version, "");
            document.FileVersion = Decimal.Parse(Tokenizer.Current.Value);

            if (header != "Microsoft Visual Studio Solution File, Format Version ")
                throw new Exception("Solution Header missing");

            ReadUntil(TokenType.InLineComment);
            document.VisualStudioVersion = ReadString().Replace("# ", "");

            _tokenizer.SetSkip(true, TokenType.WhiteSpace);
            ReadNextToken();

            while (!Tokenizer.IsNextToken("Project") && Tokenizer.Current.Type != TokenType.NewLine)
            {
                ReadNameValuePair(document.Info);
            }
        }

        private void ParseProjectSection(Project project)
        {
            var projectSection = ParseSection<ProjectSection>("EndProjectSection");
            if (projectSection != null)
                project.Section = projectSection;
        }

        private void ParseProjects(SolutionDocument document)
        {
            while (Tokenizer.TokenEquals("Project"))
            {
                var project = new Project();

                ExpectToken("(");
                project.TypeId = ReadString();
                ExpectToken(")");

                ExpectToken("=");

                project.Name = ReadString();
                ExpectToken(",");
                project.Folder = ReadString();
                ExpectToken(",");
                project.Id = ReadString();

                document.Projects.Add(project);
                ReadNextToken();

                if (Tokenizer.TokenEquals("ProjectSection"))
                    ParseProjectSection(project);

                ExpectToken("EndProject");
                ReadNextToken();
            }
        }

        private void ParseGlobals(SolutionDocument document)
        {
            if (Tokenizer.TokenEquals("Global"))
            {
                ReadNextToken();

                while (Tokenizer.TokenEquals("GlobalSection"))
                {
                    ParseGlobalSection(document);
                }
            }
        }

        private T ParseSection<T>(string endSectionToken) where T : Section, new()
        {
            var section = new T();

            ExpectToken("(");
            section.Name = ReadString();
            ExpectToken(")");

            ExpectToken("=");

            section.When = ReadString();
            ReadNextToken();

            while (!Tokenizer.TokenEquals(endSectionToken))
                ReadNameValuePair(section.Info);

            ReadNextToken();

            return section;
        }

        private void ParseGlobalSection(SolutionDocument document)
        {
            var global = ParseSection<GlobalSection>("EndGlobalSection");
            document.GlobalSections.Add(global);
        }

        public SolutionDocument Execute()
        {
            var document = new SolutionDocument();

            ParseHeader(document);
            ParseProjects(document);
            ParseGlobals(document);

            return document;
        }
    }
}
