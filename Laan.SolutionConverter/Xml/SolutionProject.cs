using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Laan.SolutionConverter.Xml
{
    [Serializable]
    public class SolutionProject
    {
        /// <summary>
        /// Initializes a new instance of the SolutionProject class.
        /// </summary>
        public SolutionProject()
        {
            Configurations = new List<NameValue>();
        }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("location")]
        public string Location { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlArray("configurations"), XmlArrayItem("configuration")]
        public List<NameValue> Configurations { get; set; }
    }
}
