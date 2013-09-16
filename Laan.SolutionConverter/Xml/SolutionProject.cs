using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Laan.SolutionConverter.Xml
{
    [Serializable]
    public class SolutionProject : SolutionItem
    {
        [XmlElement("location")]
        public string Location { get; set; }

        [XmlElement("type")]
        public string Type { get; set; }
    }
}
