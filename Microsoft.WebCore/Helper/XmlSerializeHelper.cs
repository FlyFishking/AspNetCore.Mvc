using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.WebCore.Helper
{
    public static class XmlSerializeHelper
    {
        /// <summary>
        /// Serialize obj to local xml file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="xmlFilePath">xml file to save serialized object<example>example.xml</example></param>
        /// <param name="includNameSpace">wheather or not include namespace at the header</param>
        public static void Serialize<T>(T model, string xmlFilePath, bool includNameSpace = false)
        {
            if (string.IsNullOrEmpty(xmlFilePath))
                throw new ArgumentNullException(nameof(xmlFilePath));

            var serializer = new XmlSerializer(typeof(T));
            using (var sw = File.CreateText(xmlFilePath))
            {
                if (includNameSpace)
                {
                    serializer.Serialize(sw, model);
                }
                else
                {
                    serializer.Serialize(sw, model, new XmlSerializerNamespaces());
                }
            }
        }

        /// <summary>
        /// Serialize obj to xml string
        /// </summary>
        /// <typeparam name="T">object type</typeparam>
        /// <param name="model">object to be serialized</param>
        /// <returns></returns>
        public static string SerializeToString<T>(T model)
        {
            var serializer = new XmlSerializer(typeof(T));
            string strXml = null;
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, model);
                strXml = Encoding.UTF8.GetString(stream.ToArray());
            }
            return strXml;
        }

        public static byte[] SerializeToBytes<T>(T model)
        {
            var serializer = new XmlSerializer(typeof(T));
            byte[] buffer = null;
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, model);
                buffer = stream.ToArray();
            }
            return buffer;
        }

        /// <summary>
        /// Deserialize xml file to specify object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inputUri">The URI for the file that contains the XML data. <example>D:\example.xml</example></param>
        /// <returns>instance of T</returns>
        public static T Deserialize<T>(string inputUri)
        {
            if (string.IsNullOrEmpty(inputUri))
                throw new ArgumentNullException(nameof(inputUri));
            if (!File.Exists(inputUri))
                throw new FileNotFoundException("File or Uri not found");
            using (var reader = XmlReader.Create(inputUri))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(reader);
            }
            //using (FileStream fs = new FileStream(inputUri, FileMode.Create, FileAccess.Write, FileShare.None))
            //{
            //    return XmlDeserializeObject<T>(fs);
            //}
        }

        /// <summary>
        /// Deserializes the object.
        /// </summary>
        /// <typeparam name="T">the template name.</typeparam>
        /// <param name="content">The input XML string inputUri.</param>
        /// <returns>deserialized object</returns>
        public static T DeserializeString<T>(string content)
        {
            var ser = new XmlSerializer(typeof(T));
            using (var reader = new StringReader(content))
            {
                return (T)ser.Deserialize(reader);
            }
        }

        public static T DeserializeStream<T>(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(stream);
        }
    }
}