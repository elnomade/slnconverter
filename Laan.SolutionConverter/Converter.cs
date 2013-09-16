using System;
using System.Collections.Generic;
using System.Linq;

using Laan.SolutionConverter.Xml;
using Laan.SolutionConverter.Utils;

namespace Laan.SolutionConverter
{
    public class Converter
    {
        public enum ItemType
        {
            Folder,
            Project
        }

        const string FolderTypeId = "{2150E333-8FDC-42A3-9474-1A3956D46DE8}";

        private static SolutionProject ConvertTo(Project project)
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

        public void WriteDocument(SolutionDocument document, string output)
        {
            var result = new Solution()
            {
                Version = document.FileVersion,
                VisualStudioVersion = document.VisualStudioVersion
            };

            foreach (var info in document.Info)
            {
                result.Headers.Add(new NameValue { Name = info.Key, Value = info.Value });
            }

            var solutionConfigurations = document.GlobalSections.FirstOrDefault(gs => gs.Name == "SolutionConfigurationPlatforms");
            if (solutionConfigurations != null)
                result.Configurations.AddRange(solutionConfigurations.Info.Select(i => new NameValue { Name = i.Key, Value = i.Value }));

            var solutionProperties = document.GlobalSections.FirstOrDefault(gs => gs.Name == "SolutionProperties");
            if (solutionProperties != null)
                result.Properties.AddRange(solutionProperties.Info.Select(i => new NameValue { Name = i.Key, Value = i.Value }));

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

                select new
                {
                    Folder = folders.FirstOrDefault(p => p.Id == np.Key),
                    Parent = folders.FirstOrDefault(p => p.Id == np.Value)
                }
            )
            .Where(f => f.Folder != null)
            .ToList();

            foreach (var nesting in nestedFolders.Where(nf => nf.Parent != null))
            {
                nesting.Parent.Folders.Add(nesting.Folder);
            }

            var parentedFolders = nestedFolders.Select(nf => nf.Folder).ToList();
            result.Items.Folders.AddRange(folders.Except(parentedFolders));

            var projects = document.Projects
                .Where(p => p.TypeId != FolderTypeId)
                .Select(p => ConvertTo(p))
                .ToList();

            var projectionConfigurations = document.GlobalSections.FirstOrDefault(gs => gs.Name == "ProjectConfigurationPlatforms");
            if (projectionConfigurations != null)
            {
                foreach (var project in projects)
                {
                    foreach (var info in projectionConfigurations.Info)
                    {
                        if (!info.Key.StartsWith(project.Id))
                            continue;

                        var config = info.Key.Replace(project.Id + ".", "");

                        if (project.Configurations == null)
                            project.Configurations = new List<NameValue>();
                        project.Configurations.Add(new NameValue { Name = config, Value = info.Value });
                    }
                }
            }

            var nestedProjects = (

                from np in nestingPairs
                join p in projects
                  on np.Key equals p.Id

                select new
                {
                    Project = p,
                    Folder = folders.FirstOrDefault(pr => pr.Id == np.Value)
                }

            ).ToList();

            foreach (var nesting in nestedProjects)
            {
                if (nesting.Folder != null)
                {
                    nesting.Folder.Projects.Add(nesting.Project);
                    projects.Remove(nesting.Project);
                }
            }

            foreach (var folder in folders)
            {
                var documentProject = document.Projects.First(p => p.Id == folder.Id);
                if (documentProject.Section != null)
                {
                    foreach (var info in documentProject.Section.Info)
                    {
                        if (folder.Configurations == null)
                            folder.Configurations = new List<NameValue>();

                        folder.Configurations.Add(new NameValue { Name = info.Key, Value = info.Value });
                    }
                }
            }

            result.Items.Projects.AddRange(projects.OrderBy(p => p.Name).ToList());
            Sort(result.Items);

            result.SaveAsXml(output);
        }
    }
}
