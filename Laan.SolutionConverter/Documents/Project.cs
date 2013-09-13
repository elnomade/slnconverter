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
        public string TypeId { get; set; }
        public string Name { get; set; }
        public string Folder { get; set; }
        public string Id { get; set; }
    }
}
