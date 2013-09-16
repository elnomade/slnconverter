using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Laan.SolutionConverter.Xml
{
    [XmlRoot("solution")]
    [Serializable]
    public class Solution
    {
        /// <summary>
        /// Initializes a new instance of the XmlSolution class.
        /// </summary>
        public Solution()
        {
            Properties = new List<NameValue>();
            Headers = new List<NameValue>();
            Configurations = new List<NameValue>();
            Items = new SolutionFolder();
        }

        [XmlAttribute("version")]
        public decimal Version { get; set; }

        [XmlAttribute("visualStudioVersion")]
        public string VisualStudioVersion { get; set; }

        [XmlArray("properties"), XmlArrayItem("property")]
        public List<NameValue> Properties { get; set; }

        [XmlArray("headers"), XmlArrayItem("header")]
        public List<NameValue> Headers { get; set; }

        [XmlArray("configurations"), XmlArrayItem("configuration")]
        public List<NameValue> Configurations { get; set; }

        [XmlElement("items")]
        public SolutionFolder Items { get; set; }
    }
}
