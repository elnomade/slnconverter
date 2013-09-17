using System;
using System.Collections.Generic;
using System.Linq;

using Laan.SolutionConverter.Xml;
using Laan.SolutionConverter.Utils;

namespace Laan.SolutionConverter
{
    public class SlnToXmlConverter
    {
        const string FolderTypeId = "{2150E333-8FDC-42A3-9474-1A3956D46DE8}";

        private SolutionProject ConvertTo(Project project)
        {
            return new SolutionProject
            {
                Id = project.Id,
                Name = project.Name,
                Location = project.Folder,
                Type = project.TypeId
            };
        }

        private void Sort(SolutionFolder parent)
        {
            parent.Folders = parent.Folders.OrderBy(f => f.Name).ToList();

            foreach (var folder in parent.Folders)
                Sort(folder);
        }

        private Solution CreateSolution(SolutionDocument document)
        {
            var solution = new Solution()
            {
                Version = document.FileVersion,
                VisualStudioVersion = document.VisualStudioVersion
            };

            foreach (var info in document.Info)
                solution.Headers.Add(new NameValue { Name = info.Key, Value = info.Value });

            var configurations = document.GlobalSections.FirstOrDefault(gs => gs.Name == "SolutionConfigurationPlatforms");
            if (configurations != null)
                solution.Configurations.AddRange(configurations.Info.Select(i => new NameValue { Name = i.Key, Value = i.Value }));

            var solutionProperties = document.GlobalSections.FirstOrDefault(gs => gs.Name == "SolutionProperties");
            if (solutionProperties != null)
                solution.Properties.AddRange(solutionProperties.Info.Select(i => new NameValue { Name = i.Key, Value = i.Value }));
            
            return solution;
        }

        private void CreateProjectConfiguration(GlobalSection projectionConfigurations, SolutionProject project)
        {
            foreach (var info in projectionConfigurations.Info)
            {
                if (!info.Key.StartsWith(project.Id))
                    continue;

                var config = info.Key.Replace(project.Id + ".", "");

                project.AddConfiguration(config, info.Value);
            }
        }

        private List<SolutionProject> CreateProjects(SolutionDocument solution)
        {
            var projects = solution.Projects
                .Where(p => p.TypeId != FolderTypeId)
                .Select(ConvertTo)
                .ToList();

            var configurations = solution.GlobalSections.FirstOrDefault(gs => gs.Name == "ProjectConfigurationPlatforms");
            if (configurations == null)
                return projects;

            foreach (var project in projects)
                CreateProjectConfiguration(configurations, project);

            return projects;
        }

        private void BuildFolderHeirarchy(List<SolutionFolder> folders, List<KeyValuePair<string, string>> nestingPairs, List<SolutionProject> projects)
        {
            var nestedProjects = (

                from np in nestingPairs

                join p in projects
                    on np.Key equals p.Id

                select new { Child = p, Parent = folders.FirstOrDefault(pr => pr.Id == np.Value) }

            ).ToList();

            foreach (var nestPair in nestedProjects.Where(p => p.Parent != null))
            {
                nestPair.Parent.Projects.Add(nestPair.Child);
                projects.Remove(nestPair.Child);
            }
        }

        private void WriteSolutionFolders(SolutionDocument document, List<SolutionFolder> folders)
        {
            foreach (var folder in folders)
            {
                var documentProject = document.Projects.First(p => p.Id == folder.Id);
                if (documentProject.Section != null)
                {
                    foreach (var info in documentProject.Section.Info)
                    {
                        if (folder.Configurations == null)
                            folder.Configurations = new List<NameValue>();

                        folder.AddConfiguration(info.Key, info.Value);
                    }
                }
            }
        }

        public void WriteDocument(SolutionDocument document, string output)
        {
            Solution result = CreateSolution(document);

            var folders = document.Projects
                .Where(p => p.TypeId == FolderTypeId)
                .Select(p => new SolutionFolder { Id = p.Id, Name = p.Name })
                .ToList();

            var nestingPairs = document.GlobalSections
                .Where(gs => gs.Name == "NestedProjects")
                .SelectMany(gs => gs.Info)
                .ToList();

            var nestedFolders = (

                from np in nestingPairs

                join f in folders
                  on np.Value equals f.Id

                let folder = folders.FirstOrDefault(p => p.Id == np.Key)
                let parent = folders.FirstOrDefault(p => p.Id == np.Value)

                where folder != null

                select new { Folder = folder, Parent = parent }
            )
            .ToList();

            foreach (var nesting in nestedFolders.Where(nf => nf.Parent != null))
                nesting.Parent.Folders.Add(nesting.Folder);
            
            var parentedFolders = nestedFolders.Select(nf => nf.Folder).ToList();
            result.Items.Folders.AddRange(folders.Except(parentedFolders));

            var projects = CreateProjects(document);
            
            BuildFolderHeirarchy(folders, nestingPairs, projects);
            WriteSolutionFolders(document, folders);

            result.Items.Projects.AddRange(projects.OrderBy(p => p.Name).ToList());
            Sort(result.Items);

            result.SaveAsXml(output);
        }
    }
}
