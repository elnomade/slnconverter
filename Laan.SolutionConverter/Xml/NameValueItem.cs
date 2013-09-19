using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Laan.SolutionConverter.Xml
{
    [Serializable]
    public class NameValueItem
    {
        /// <summary>
        /// Initializes a new instance of the ConfigurationItem class.
        /// </summary>
        public NameValueItem()
        {
            Items = new List<NameValue>();
        }

        public void AddItem(string config, string value)
        {
            Items.Add(new NameValue { Name = config, Value = value });
        }

        public bool Any()
        {
            return Items.Any();
        }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("configuration")]
        public List<NameValue> Items { get; set; }

        [XmlAttribute("when")]
        public string When { get; set; }
    }
}
