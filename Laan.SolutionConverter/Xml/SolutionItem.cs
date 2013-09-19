using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Laan.SolutionConverter.Xml
{
    [Serializable]
    public abstract class SolutionItem
    {
        /// <summary>
        /// Initializes a new instance of the SolutionItem class.
        /// </summary>
        public SolutionItem()
        {
        }

        public void AddConfiguration(string name, string when, string config, string value)
        {
            if (Configuration == null)
                Configuration = new NameValueItem() { Name = name, When = when };

            Configuration.AddItem(config, value);
        }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlElement("location")]
        public string Location { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlElement("configurations")]
        public NameValueItem Configuration { get; set; }
    }
}
