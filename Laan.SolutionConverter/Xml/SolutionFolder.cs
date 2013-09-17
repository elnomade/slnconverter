using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Laan.SolutionConverter.Xml
{
    [Serializable]
    public class SolutionFolder : SolutionItem
    {
        /// <summary>
        /// Initializes a new instance of the SolutionFolder class.
        /// </summary>
        public SolutionFolder()
        {
            Folders = new List<SolutionFolder>();
            Projects = new List<SolutionProject>();
        }

        [XmlElement("folder")]
        public List<SolutionFolder> Folders { get; set; }

        [XmlElement("project")]
        public List<SolutionProject> Projects { get; set; }
    }
}
