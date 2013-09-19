using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Laan.SolutionConverter.Utils;
using Laan.SolutionConverter.Xml;
using System.Text;

namespace Laan.SolutionConverter
{
    public class XmlToSlnConverter
    {
        private Solution _document;

        private string GetProjectsOutput(SolutionItem item)
        {
            var output = new StringBuilder();

            output.AppendFormat("Project(\"{0}\") = \"{1}\", \"{2}\", \"{3}\"\n", item.Type ?? SlnToXmlConverter.FolderTypeId, item.Name, item.Location ?? item.Name, item.Id);

            if (item.Configuration != null && item.Configuration.Name != "ProjectConfigurationPlatforms" && item.Configuration.Any())
            {
                output.AppendFormat("\tProjectSection({0}) = {1}\n", item.Configuration.Name, item.Configuration.When);
                foreach (NameValue configuration in item.Configuration.Items)
                {
                    output.AppendFormat("\t\t{0} = {1}\n", configuration.Name, configuration.Value);
                }
                output.AppendLine("\tEndProjectSection");
            }

            output.Append("EndProject");
            return output.ToString();
        }

        private void AddPairs(List<Tuple<SolutionItem, SolutionItem>> pairs, SolutionFolder parent)
        {
            foreach (var child in parent.Projects.Union(parent.Folders.OfType<SolutionItem>()))
            {
                pairs.Add(new Tuple<SolutionItem, SolutionItem>(child, parent));
                var childFolder = child as SolutionFolder;
                if (childFolder != null)
                    AddPairs(pairs, childFolder);
            }
        }

        private void OutputNestedProjects(Solution solution, StringBuilder output)
        {
            var pairs = new List<Tuple<SolutionItem, SolutionItem>>();
            AddPairs(pairs, solution.Items);

            if (!pairs.Any())
                return;

            output.AppendLine("\tGlobalSection(NestedProjects) = preSolution");
            foreach (var pair in pairs.Where(p => p.Item2.Id != null))
            {
                output.AppendFormat("\t\t{0} = {1}\n", pair.Item1.Id, pair.Item2.Id);
            }
            output.AppendLine("\tEndGlobalSection");
        }

        private void OutputProjectsConfigurations(Solution solution, StringBuilder output)
        {
            var projects = solution.Items.Folders
                .Recurse(p => p.Folders)
                .SelectMany(f => f.Projects)
                .Union(solution.Items.Projects)
                .ToList();

            if (!projects.Any())
                return;

            output.AppendLine("\tGlobalSection(ProjectConfigurationPlatforms) = postSolution");
            foreach (SolutionProject project in projects)
            {
                foreach (var config in project.Configuration.Items)
                {
                    output.AppendFormat("\t\t{0}.{1} = {2}\n", project.Id, config.Name, config.Value);
                }
            }
            output.AppendLine("\tEndGlobalSection");
        }

        private string GetSectionsOutput(Solution solution)
        {
            var output = new StringBuilder();

            output.AppendLine("Global");

            foreach (var item in solution.Sections)
            {
                output.AppendFormat("\tGlobalSection({0}) = {1}\n", item.Name, item.When);
                foreach (NameValue configuration in item.Items)
                {
                    output.AppendFormat("\t\t{0} = {1}\n", configuration.Name, configuration.Value);
                }
                output.AppendLine("\tEndGlobalSection");
            }

            OutputNestedProjects(solution, output);
            OutputProjectsConfigurations(solution, output);

            output.Append("EndGlobal");

            return output.ToString();
        }

        public void WriteDocument(string path, string outputName)
        {
            var xml = File.ReadAllText(path);
            _document = xml.FromXml<Solution>();

            var lines = new List<string>();

            lines.Add(String.Format("Microsoft Visual Studio Solution File, Format Version {0}", _document.Version));
            lines.Add(String.Format("# {0}", _document.VisualStudioVersion));
            if (_document.Headers != null)
                lines.AddRange(_document.Headers.Select(h => String.Format("{0} = {1}", h.Name, h.Value)));

            var all = _document.Items.Folders.Recurse(si => si.Folders).ToList();
            var items = _document.Items.Projects.Union(all.SelectMany(a => a.Projects)).OfType<SolutionItem>().ToList();

            lines.AddRange(items.Union(all).Select(f => GetProjectsOutput(f)));

            lines.Add(GetSectionsOutput(_document));

            File.WriteAllLines(outputName, lines.ToArray());
        }
    }
}
