using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Laan.SolutionConverter.Utils
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Recurse<T>(this IEnumerable<T> enumerable, Func<T, IEnumerable<T>> childNodesGenerator)
        {
            foreach (T item in enumerable)
            {
                foreach (T childNode in childNodesGenerator(item).Recurse(childNodesGenerator))
                {
                    yield return childNode;
                }
                yield return item;
            }
        }
    }
        
    public static class XmlExtensions
    {
        /// <summary>
        /// Return XML representation of object
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <param name="encoding">Text encoding to use</param>
        /// <param name="omitXmlDeclaration">true if XML declaration should be ommitted</param>
        /// <returns></returns>
        public static string ToXml(this object entity)
        {
            var writerSettings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                OmitXmlDeclaration = false,
                Indent = true,
            };

            using (var stream = new MemoryStream())
            {
                using (var xmlWriter = XmlWriter.Create(stream, writerSettings))
                {
                    var serializer = new XmlSerializer(entity.GetType());
                    var namespaces = new XmlSerializerNamespaces();
                    namespaces.Add("", "");
                    serializer.Serialize(xmlWriter, entity, namespaces);

                    xmlWriter.Flush();
                }

                stream.Position = 0;

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private static void EnsurePath(string fileName)
        {
            string path = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        public static void SaveAsXml(this object entity, string fileName)
        {
            EnsurePath(fileName);
            File.WriteAllText(fileName, entity.ToXml());
        }

        public static T FromXml<T>(this string xmlString) where T : class
        {
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                object deserializedObject;
                using (var reader = new StringReader(xmlString))
                {
                    deserializedObject = serializer.Deserialize(reader);
                }
                return (T)Convert.ChangeType(deserializedObject, typeof(T));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        public static T FromXml<T>(this byte[] data, Encoding encoding) where T : class
        {
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                object deserializedObject;

                using (var stream = new MemoryStream(data))
                {
                    deserializedObject = serializer.Deserialize(stream);
                }
                return (T)Convert.ChangeType(deserializedObject, typeof(T));
            }
            catch
            {
                return null;
            }
        }

        public static T FromXml<T>(this FileInfo file) where T : class
        {
            EnsurePath(file.FullName);
            return File.ReadAllText(file.FullName).FromXml<T>();
        }
    }

}
