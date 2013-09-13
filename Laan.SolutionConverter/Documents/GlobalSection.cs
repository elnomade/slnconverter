using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Laan.SolutionConverter
{
    [DebuggerDisplay("GlobalSection({Name}) = {Timing}")]
    public class GlobalSection
    {
        /// <summary>
        /// Initializes a new instance of the GlobalSection class.
        /// </summary>
        public GlobalSection()
        {
            Info = new Dictionary<string, string>();
        }

        public string Name { get; set; }
        public string Timing { get; set; } // Pre / Post
        public Dictionary<string, string> Info { get; set; }
    }
}
