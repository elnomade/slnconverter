using Laan.SolutionConverter.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Laan.SolutionConverter
{
    public class Converter
    {
        public Converter()
        {

        }

        public void WriteDocument(SolutionDocument document, string output)
        {
            var result = new Solution() 
            { 
                Version = document.FileVersion, 
                VisualStudioVersion = document.VisualStudioVersion 
            };

            foreach (var info in document.Info)
            {
                result.Headers.Add(new NameValue { Name = info.Key, Value = info.Value });
            }
        }
    }
}
