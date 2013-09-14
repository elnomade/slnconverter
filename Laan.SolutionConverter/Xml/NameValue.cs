using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Laan.SolutionConverter.Xml
{
    [Serializable]
    public class NameValue
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }
    }
}
