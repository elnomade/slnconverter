using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Laan.SolutionConverter
{
    [DebuggerDisplay("Project({TypeId}) = {Name}, {Folder}, {Id}")]
    public class Project
    {
        /// <summary>
        /// Initializes a new instance of the Project class.
        /// </summary>
        public Project()
        {
        }

        public string TypeId { get; set; }
        public string Name { get; set; }
        public string Folder { get; set; }
        public string Id { get; set; }
        public ProjectSection Section { get; set; }
    }
}
