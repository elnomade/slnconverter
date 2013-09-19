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
            Sections = new List<NameValueItem>();
            Items = new SolutionFolder();
        }

        [XmlAttribute("version")]
        public decimal Version { get; set; }

        [XmlAttribute("visualStudioVersion")]
        public string VisualStudioVersion { get; set; }

        [XmlArray("headers"), XmlArrayItem("header")]
        public List<NameValue> Headers { get; set; }

        [XmlElement("items")]
        public SolutionFolder Items { get; set; }

        [XmlArray("sections"), XmlArrayItem("section")]
        public List<NameValueItem> Sections { get; set; }
    }
}
