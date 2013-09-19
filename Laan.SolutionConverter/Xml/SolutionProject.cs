using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;

namespace Laan.SolutionConverter.Xml
{
    [Serializable]
    public class ProjectDependency
    {
        [XmlElement("project")]
        public string Name { get; set; }
    }

    [Serializable]
    [DebuggerDisplay("Project: {Name}")]
    public class SolutionProject : SolutionItem
    {
        public void AddDependency(string value)
        {
            if (Dependencies == null)
                Dependencies = new List<ProjectDependency>();

            Dependencies.Add(new ProjectDependency { Name = value });
        }

        [XmlElement("dependsOn")]
        public List<ProjectDependency> Dependencies { get; set; }
    }
}
