using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Laan.SolutionConverter
{
    public abstract class Section
    {
        /// <summary>
        /// Initializes a new instance of the GlobalSection class.
        /// </summary>
        public Section()
        {
            Info = new Dictionary<string, string>();
        }

        public string Name { get; set; }
        public string When { get; set; } // Pre / Post
        public Dictionary<string, string> Info { get; set; }
    }

    [DebuggerDisplay("GlobalSection({Name}) = {Timing}")]
    public class GlobalSection : Section
    {
    }

    [DebuggerDisplay("ProjectSection({Name}) = {Timing}")]
    public class ProjectSection : Section
    {
    }
}
