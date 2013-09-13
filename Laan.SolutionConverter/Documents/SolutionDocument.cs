using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Laan.SolutionConverter
{
    public class SolutionDocument
    {
        /// <summary>
        /// Initializes a new instance of the SolutionDocument class.
        /// </summary>
        public SolutionDocument()
        {
            Info = new Dictionary<string, string>();
            Projects = new List<Project>();
            GlobalSections = new List<GlobalSection>();
        }

        public string VisualStudioVersion { get; set; }
        public decimal FileVersion { get; set; }
        public Dictionary<string, string> Info { get; set; }
        public IList<Project> Projects { get; set; }
        public IList<GlobalSection> GlobalSections { get; set; }
    }
}
